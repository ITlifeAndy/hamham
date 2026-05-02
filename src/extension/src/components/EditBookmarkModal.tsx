import React, { useState } from 'react';
import { bookmarkApi } from '../api/bookmarks';
import type { Bookmark } from '../api/types';
import { HexColorPicker } from 'react-colorful';

interface EditBookmarkModalProps {
  isOpen: boolean;
  onClose: () => void;
  onBookmarkUpdated: () => void;
  bookmark: Bookmark;
}

export const EditBookmarkModal: React.FC<EditBookmarkModalProps> = ({ isOpen, onClose, onBookmarkUpdated, bookmark }) => {
  const [title, setTitle] = useState(bookmark.title);
  const [subtitle, setSubtitle] = useState(bookmark.subtitle || '');
  const [url, setUrl] = useState(bookmark.url);
  const [color, setColor] = useState(bookmark.color || '#dee1ff');
  // const [isFavorite, setIsFavorite] = useState(bookmark.isFavorite);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const isColorChanged = color !== (bookmark.color || '#dee1ff');
  const mustSave = isColorChanged && (color === 'glass' || color === '#dee1ff');

  if (!isOpen) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!title.trim() || !url.trim()) {
      setError('名稱與 URL 為必填');
      return;
    }

    setLoading(true);
    setError('');
      try {
         await bookmarkApi.updateBookmark(bookmark.id, {
           title,
           subtitle,
           url,
           categoryId: bookmark.categoriesId,
           color,
           // isFavorite,
         } as any);
        onBookmarkUpdated();
        onClose();
      } catch (err) {
      setError('更新書籤失敗，請稍後再試');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 z-[100] flex items-center justify-center p-4 bg-black/40 backdrop-blur-sm">
      <div className="bg-white rounded-3xl w-full max-w-md shadow-2xl overflow-hidden animate-in fade-in zoom-in duration-200">
        <div className="px-6 py-4 border-b border-slate-100 flex justify-between items-center">
          <h2 className="font-bold text-lg text-slate-900">編輯書籤</h2>
          <button 
            onClick={onClose} 
            disabled={mustSave}
            className={`p-1 rounded-full transition-colors ${mustSave ? 'opacity-30 cursor-not-allowed' : 'hover:bg-slate-100'}`}
          >
            <span className="material-symbols-outlined text-slate-500">close</span>
          </button>
        </div>

        <form onSubmit={handleSubmit} className="p-6 space-y-6">
          <div>
            <label className="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">名稱 *</label>
            <input
              type="text"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              className="w-full px-4 py-2 rounded-xl border border-slate-200 focus:border-primary focus:ring-2 focus:ring-primary/20 outline-none transition-all"
            />
          </div>

          <div>
            <label className="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">副標題</label>
            <input
              type="text"
              value={subtitle}
              onChange={(e) => setSubtitle(e.target.value)}
              className="w-full px-4 py-2 rounded-xl border border-slate-200 focus:border-primary focus:ring-2 focus:ring-primary/20 outline-none transition-all"
            />
          </div>

           <div>
             <label className="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">URL *</label>
             <input
               type="url"
               value={url}
               onChange={(e) => setUrl(e.target.value)}
               className="w-full px-4 py-2 rounded-xl border border-slate-200 focus:border-primary focus:ring-2 focus:ring-primary/20 outline-none transition-all"
             />
           </div>
           {/* <div className="flex items-center gap-2">
             <input 
               type="checkbox" 
               id="isFavorite" 
               checked={isFavorite} 
               onChange={(e) => setIsFavorite(e.target.checked)} 
               className="w-4 h-4 rounded border-slate-300 text-primary focus:ring-primary"
             />
             <label htmlFor="isFavorite" className="text-sm text-slate-600 cursor-pointer select-none">標記為最愛</label>
           </div> */}


            <div className="flex flex-col gap-6">
              <div className="flex flex-col gap-3">
                <label className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">顏色</label>
                <div className="flex flex-col gap-3 items-center bg-slate-50 border rounded-2xl p-3">
                  <div className="flex gap-3 w-full justify-center mb-2">
                    <button 
                      type="button"
                      onClick={() => setColor('#dee1ff')}
                      className={`px-3 py-1.5 rounded-full text-[10px] font-bold transition-all ${color === '#dee1ff' ? 'bg-primary text-white shadow-md' : 'bg-slate-100 text-slate-500 hover:bg-slate-200'}`}
                    >
                      預設色
                    </button>
                    <button 
                      type="button"
                      onClick={() => setColor('glass')}
                      className={`px-3 py-1.5 rounded-full text-[10px] font-bold transition-all ${color === 'glass' ? 'bg-blue-400 text-white shadow-md' : 'bg-slate-100 text-slate-500 hover:bg-slate-200'}`}
                    >
                      ✨ 玻璃效果
                    </button>
                  </div>
                  {color !== 'glass' ? (
                    <>
                      <div className="scale-90 origin-top">
                        <HexColorPicker color={color} onChange={setColor} />
                      </div>
                      <div className="flex items-center gap-2 w-full justify-center">
                        <div className="w-4 h-4 rounded-full border shadow-sm" style={{ backgroundColor: color }} />
                        <input 
                          type="text" 
                          value={color} 
                          onChange={(e) => setColor(e.target.value)} 
                          className="text-[10px] w-24 px-2 py-1 rounded-lg border border-slate-200 outline-none focus:ring-1 focus:ring-primary/20 transition-all"
                        />
                      </div>
                    </>
                  ) : (
                    <div className="w-full p-4 rounded-xl border border-blue-100 bg-blue-50/50 text-center">
                      <p className="text-[10px] text-blue-600 italic">已啟用玻璃擬態效果</p>
                    </div>
                  )}
                </div>
              </div>
            </div>


          {error && <p className="text-red-500 text-xs text-center">{error}</p>}

          <div className="flex gap-3 pt-4">
           <button
             type="button"
             onClick={onClose}
             disabled={mustSave}
             className={`flex-1 px-4 py-2 rounded-xl border border-slate-200 text-slate-600 font-semibold transition-all active:scale-95 ${mustSave ? 'opacity-30 cursor-not-allowed' : 'hover:bg-slate-50'}`}
           >
             取消
           </button>
           <button
             type="submit"
             disabled={loading}
             className="flex-1 px-4 py-2 rounded-xl bg-[#2f4dd5] text-white font-semibold hover:bg-blue-600 transition-all active:scale-95 disabled:opacity-50"
           >
             {loading ? '更新中...' : '儲存變更'}
           </button>
          </div>
        </form>
      </div>
    </div>
  );
};
