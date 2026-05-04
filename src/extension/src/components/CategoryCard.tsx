import React, { useEffect, useState } from 'react';
import type { Category, Bookmark } from '../api/types';
import { bookmarkApi } from '../api/bookmarks';
import { AddCategoryModal } from './AddCategoryModal';
import { SubCategoryCard } from './SubCategoryCard';
import { useWallpaper } from '../providers/WallpaperProvider';

const getContrastColor = (hexColor: string) => {
  if (hexColor === 'glass') return 'light';
  const color = hexColor.replace('#', '');
  if (color.length === 3) {
    const r = parseInt(color[0] + color[0], 16);
    const g = parseInt(color[1] + color[1], 16);
    const b = parseInt(color[2] + color[2], 16);
    return (0.299 * r + 0.587 * g + 0.114 * b) > 128 ? 'light' : 'dark';
  }
  if (color.length === 6) {
    const r = parseInt(color.substring(0, 2), 16);
    const g = parseInt(color.substring(2, 4), 16);
    const b = parseInt(color.substring(4, 6), 16);
    return (0.299 * r + 0.587 * g + 0.114 * b) > 128 ? 'light' : 'dark';
  }
  return 'light';
};

interface CategoryCardProps {
  category: Category;
  onCategoryUpdated?: () => void;
  setEditingCategory: (cat: Category | null) => void;
  setEditingBookmark: (bm: Bookmark | null) => void;
  refreshTrigger?: number;
  openAddBookmark: (catId: string) => void;
}

interface UnifiedItem {
  itemId: string;
  type: 'Bookmark' | 'Category';
  sortOrder: number;
  data: any;
}

export const CategoryCard: React.FC<CategoryCardProps> = ({ category, onCategoryUpdated, setEditingCategory, setEditingBookmark, refreshTrigger, openAddBookmark }) => {
  console.log(`[CategoryCard] Rendering ${category.name}, color: ${category.color}`);
  const [unifiedItems, setUnifiedItems] = useState<UnifiedItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [isAddCategoryModalOpen, setIsAddCategoryModalOpen] = useState(false);
  const [showAddMenu, setShowAddMenu] = useState(false);
  const [draggedItemId, setDraggedItemId] = useState<{ id: string, type: 'Bookmark' | 'Category' } | null>(null);


  const { isDarkWallpaper } = useWallpaper();
  const contrast = getContrastColor(category.color || '#dee1ff');
  const textColor = category.color === 'glass' 
    ? (isDarkWallpaper ? 'text-white' : 'text-slate-900') 
    : (contrast === 'light' ? 'text-slate-900' : 'text-white');
  const iconColor = category.color === 'glass' 
    ? (isDarkWallpaper ? 'text-white' : 'text-primary') 
    : (contrast === 'light' ? 'text-primary' : 'text-white');
  const hoverBg = contrast === 'light' ? 'hover:bg-black/10' : 'hover:bg-white/20';

  useEffect(() => {
    refreshData();
  }, [category.id, refreshTrigger]);

  const refreshData = async () => {
    setLoading(true);
    try {
      const items = await bookmarkApi.getUnifiedItems(category.id);
      setUnifiedItems(items);
    } catch (err) {
      console.error(`Failed to load data for ${category.name}`, err);
    } finally {
      setLoading(false);
    }
  };

  const onDragStart = (e: React.DragEvent, id: string, type: 'Bookmark' | 'Category') => {
    e.stopPropagation();
    setDraggedItemId({ id, type });
  };

  // Use a ref to always have access to the latest unifiedItems without closure issues in async functions
  const itemsRef = React.useRef<UnifiedItem[]>([]);

  useEffect(() => {
    itemsRef.current = unifiedItems;
  }, [unifiedItems]);

  const onDragOver = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
  };

  const onDrop = async (e: React.DragEvent, targetId: string, _targetType: 'Bookmark' | 'Category', currentCatId: string) => {
    e.preventDefault();
    e.stopPropagation();
    
    console.log('onDrop triggered:', { targetId, currentCatId, draggedItemId });

    if (!draggedItemId || (draggedItemId.id === targetId)) return;

    const getParentId = (item: UnifiedItem) => {
      if (!item.data) return null;
      // Try all possible parent id keys as the API/Entity might vary
      return item.data.categoriesId || item.data.parentId || item.data.categoryId;
    };

    const currentItems = itemsRef.current;
    const directChildren = currentItems
      .filter(item => getParentId(item) === currentCatId)
      .sort((a, b) => a.sortOrder - b.sortOrder);

    console.log('Direct children for category', currentCatId, directChildren);

    const draggedIdx = directChildren.findIndex(i => i.itemId === draggedItemId!.id);
    const targetIdx = directChildren.findIndex(i => i.itemId === targetId);

    if (draggedIdx === -1 || targetIdx === -1) {
      console.error('Drag and drop failed: item not found in current category', {
        draggedId: draggedItemId!.id,
        targetId,
        currentCatId,
        childrenIds: directChildren.map(c => c.itemId)
      });
      return;
    }

    const updatedList = [...directChildren];
    const [draggedItem] = updatedList.splice(draggedIdx, 1);
    updatedList.splice(targetIdx, 0, draggedItem);

    try {
      await bookmarkApi.updateUnifiedOrder(currentCatId, updatedList.map((item, index) => ({
        itemId: item.itemId,
        type: item.type,
        sortOrder: index
      })));
      console.log('API update successful');
    } catch (err) {
      console.error('Failed to update unified order', err);
      refreshData();
      return;
    }

    setUnifiedItems(prevItems => 
      prevItems.map(item => {
        if (getParentId(item) === currentCatId) {
          const newIdx = updatedList.findIndex(i => i.itemId === item.itemId);
          return { ...item, sortOrder: newIdx };
        }
        return item;
      })
    );
    
    setDraggedItemId(null);
  };


  const openAddBookmarkInternal = (catId: string) => {
    openAddBookmark(catId);
  };

  const renderBookmark = (bm: any) => {
    const isGlass = bm?.color === 'glass';
    const bookmarkContrast = getContrastColor(bm?.color || '#f4f2fe');
    const bmTextColor = isGlass 
      ? (isDarkWallpaper ? 'text-white' : 'text-slate-900') 
      : (bookmarkContrast === 'light' ? 'text-slate-900' : 'text-white');
    const title = bm?.title || bm?.name || '無名稱';

    return (
      <div 
        key={bm?.id} 
        draggable
        onDragStart={(e) => onDragStart(e, bm?.id, 'Bookmark')}
        onDragOver={onDragOver}
        onDrop={(e) => onDrop(e, bm?.id, 'Bookmark', bm?.categoriesId)}
        onClick={() => window.location.href = bm?.url}
        className={`p-1 rounded-xl flex items-center gap-2 border transition-transform hover:-translate-y-0.5 active:scale-95 group cursor-move ${
          isGlass 
          ? 'bg-white/20 border-white/30 backdrop-blur-sm hover:bg-white/40' 
          : 'border-border-ring'
        }`}
        style={{ backgroundColor: isGlass ? 'transparent' : (bm?.color || '#f4f2fe') }}
      >
        <div className="flex-1 overflow-hidden">
            <h3 className={`font-bold text-sm truncate ${bmTextColor}`}>{title}</h3>
            <p className={`text-xs truncate mb-0.5 ${isGlass ? (isDarkWallpaper ? 'text-white/80' : 'text-slate-500') : 'text-text-secondary'}`}>
              {bm?.subtitle || bm?.url}
            </p>
          </div>
          <div className="flex items-center gap-1 opacity-0 group-hover:opacity-100 transition-opacity">

           <button 
             onClick={(e) => {
               e.stopPropagation();
               setEditingBookmark(bm);
             }}
             className={`p-1 ${isGlass ? (isDarkWallpaper ? 'text-white/70 hover:text-white' : 'text-slate-400 hover:text-primary') : 'text-slate-400 hover:text-primary'}`}
             title="編輯書籤"
           >
             <span className="material-symbols-outlined text-base">edit</span>
           </button>
           <button 
             onClick={(e) => {
               e.stopPropagation();
               if (confirm('確定要刪除此書籤嗎？')) {
                 bookmarkApi.deleteBookmark(bm.id).then(() => refreshData());
               }
             }}
             className={`p-1 ${isGlass ? (isDarkWallpaper ? 'text-red-300 hover:text-red-400' : 'text-slate-400 hover:text-red-500') : 'text-slate-400 hover:text-red-500'}`}
             title="刪除書籤"
           >
             <span className="material-symbols-outlined text-base">delete</span>
           </button>
         </div>
      </div>
    );
  };

  return (
    <div 
      className={`p-1 rounded-2xl border shadow-sm transition-all hover:shadow-md h-fit ${
        category.color === 'glass' 
        ? 'border-white/40 bg-white/30 backdrop-blur-md backdrop-saturate-150' 
        : 'border-white/40'
      }`}
      style={{ backgroundColor: category.color === 'glass' ? 'transparent' : (category.color || '#dee1ff') }}
    >

      <div className="flex justify-between items-center mb-4">
        <div className="flex items-center gap-2">
          <i className={`${(category as any).icon || 'fa-solid fa-folder'} ${iconColor} text-xl`}></i>
          <h2 className={`font-feature-label text-base ${textColor}`}>{category.name}</h2>
        </div>
         <div className="flex items-center gap-1 relative">
              <button 
                onClick={() => setShowAddMenu(!showAddMenu)}
                className={`p-1 ${hoverBg} rounded-full transition-colors ${iconColor}`}
                title="新增"
              >
                <span className="material-symbols-outlined text-lg">add</span>
              </button>
              <button 
                onClick={() => setEditingCategory(category)}
                className={`p-1 ${hoverBg} rounded-full transition-colors ${iconColor}`}
                title="編輯類別"
              >
                <span className="material-symbols-outlined text-lg">edit</span>
              </button>
              <button 
                onClick={() => {
                  if (confirm(`確定要刪除類別 ${category.name} 及其所有內容嗎？`)) {
                    bookmarkApi.deleteCategory(category.id).then(() => {
                      onCategoryUpdated?.();
                    });
                  }
                }}
                className={`p-1 ${hoverBg} rounded-full transition-colors text-red-500 hover:bg-red-100`}
                title="刪除類別"
              >
                <span className="material-symbols-outlined text-lg">delete</span>
              </button>
 
              {showAddMenu && (
                <div className="absolute top-full right-0 mt-1 w-40 bg-white border border-slate-200 rounded-xl shadow-xl py-1 z-[60] animate-in fade-in zoom-in-95 duration-100">
                   <button 
                     onClick={() => openAddBookmark(category.id)}
                     className="w-full text-left px-4 py-2 text-sm text-slate-600 hover:bg-slate-50 transition-colors flex items-center gap-2"
                   >
                     <span className="material-symbols-outlined text-lg text-slate-400">link</span>
                     新增書籤
                   </button>
                  <button 
                    onClick={() => {
                      setIsAddCategoryModalOpen(true);
                      setShowAddMenu(false);
                    }}
                    className="w-full text-left px-4 py-2 text-sm text-slate-600 hover:bg-slate-50 transition-colors flex items-center gap-2"
                  >
                    <span className="material-symbols-outlined text-lg text-slate-400">create_new_folder</span>
                    新增子類別
                  </button>
                </div>
              )}
            </div>

        </div>
        
          <div className="flex flex-col gap-3">
            {loading ? (
              <div className="text-xs text-text-secondary italic">Loading...</div>
            ) : (
              <>
                 {unifiedItems
                   .filter(item => item.data.categoriesId === category.id)
                   .sort((a, b) => a.sortOrder - b.sortOrder)
                   .map(item => {
                     if (item.type === 'Bookmark') return renderBookmark(item.data);
                      if (item.type === 'Category') return (
                        <SubCategoryCard 
                          key={item.itemId}
                          sub={item.data}
                          unifiedItems={unifiedItems}
                          onRefresh={refreshData}
                          onAddBookmark={openAddBookmarkInternal}
                          renderBookmark={renderBookmark}
                          onDragStart={onDragStart}
                          onDragOver={onDragOver}
                          onDrop={onDrop}
                          onEditCategory={setEditingCategory}
                        />
                      );
                     return null;
                   })}

                
                {unifiedItems.filter(item => item.data.categoriesId === category.id).length === 0 && (
                  <div className="text-xs text-text-secondary italic">No bookmarks here yet.</div>
                )}
              </>
            )}
          </div>

          <AddCategoryModal 
            isOpen={isAddCategoryModalOpen} 
            onClose={() => setIsAddCategoryModalOpen(false)} 
            onCategoryAdded={refreshData} 
            defaultParentId={category.id}
          />
        </div>
    );
  };
