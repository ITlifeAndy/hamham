import React, { useState, useEffect } from 'react';
import { createPortal } from 'react-dom';
import { bookmarkApi } from '../api/bookmarks';
import { HexColorPicker } from 'react-colorful';
import { IconPicker } from './IconPicker';
import type { Category } from '../api/types';

interface AddCategoryModalProps {
  isOpen: boolean;
  onClose: () => void;
  onCategoryAdded: () => void;
  defaultParentId?: string;
}

export const AddCategoryModal: React.FC<AddCategoryModalProps> = ({ isOpen, onClose, onCategoryAdded, defaultParentId }) => {
  const [name, setName] = useState('');
  const [color, setColor] = useState('#dee1ff');
  const [icon, setIcon] = useState('fa-solid fa-folder');
  const [parentId, setParentId] = useState<string>('');
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    if (isOpen) {
      bookmarkApi.getCategories().then(cats => {
        setCategories(cats);
        if (defaultParentId) {
          setParentId(defaultParentId);
        }
      });
    }
  }, [isOpen, defaultParentId]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!name.trim()) {
      setError('名稱為必填');
      return;
    }

    setLoading(true);
    setError('');
    try {
      await bookmarkApi.createCategory({ 
        name, 
        color, 
        icon: icon as any, 
        parentId: parentId || undefined 
      });
      onCategoryAdded();
      onClose();
      setName('');
      setColor('#dee1ff');
      setIcon('fa-solid fa-folder');
      setParentId('');
    } catch (err) {
      setError('建立類別失敗，請稍後再試');
    } finally {
      setLoading(false);
    }
  };

  if (!isOpen) return null;

  return createPortal(
    <div className="fixed inset-0 z-[1000] flex items-center justify-center p-4 bg-black/40 backdrop-blur-sm">
      <div className="bg-white rounded-3xl w-full max-w-md shadow-2xl overflow-hidden animate-in fade-in zoom-in duration-200">
        <div className="px-6 py-4 border-b border-slate-100 flex justify-between items-center">
          <h2 className="font-bold text-lg text-slate-900">新增類別</h2>
          <button onClick={onClose} className="p-1 hover:bg-slate-100 rounded-full transition-colors">
            <span className="material-symbols-outlined text-slate-500">close</span>
          </button>
        </div>
        <form onSubmit={handleSubmit} className="p-6 space-y-6">
          <div>
            <label className="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">類別名稱 *</label>
            <input
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              placeholder="例如：工作, 學習, 購物..."
              className="w-full px-4 py-2 rounded-xl border border-slate-200 focus:border-primary focus:ring-2 focus:ring-primary/20 outline-none transition-all"
            />
            {error && <p className="text-red-500 text-xs mt-1">{error}</p>}
          </div>
          <div>
            <label className="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">父類別 (可選)</label>
            <select
              value={parentId}
              onChange={(e) => setParentId(e.target.value)}
              className="w-full px-4 py-2 rounded-xl border border-slate-200 focus:border-primary focus:ring-2 focus:ring-primary/20 outline-none transition-all"
            >
              <option value="">無 (根類別)</option>
              {categories.map(cat => (
                <option key={cat.id} value={cat.id}>{cat.name}</option>
              ))}
            </select>
          </div>
          {(!parentId) && (
            <div>
              <label className="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">選擇顏色</label>
              <div className="flex flex-col gap-3 items-center">
                <div className="border rounded-xl p-2 bg-slate-50">
                  <HexColorPicker color={color} onChange={setColor} />
                </div>
                <div className="flex items-center gap-2">
                  <div className="w-4 h-4 rounded-full border" style={{ backgroundColor: color }} />
                  <input 
                    type="text" 
                    value={color} 
                    onChange={(e) => setColor(e.target.value)} 
                    className="text-[10px] w-20 px-2 py-1 rounded border border-slate-200 outline-none"
                  />
                </div>
              </div>
            </div>
          )}
          {(!parentId) && (
            <div>
              <label className="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">選擇圖標</label>
              <IconPicker value={icon} onChange={setIcon} />
            </div>
          )}
          <div className="flex gap-3 pt-4">
            <button
              type="button"
              onClick={onClose}
              className="flex-1 px-4 py-2 rounded-xl border border-slate-200 text-slate-600 font-semibold hover:bg-slate-50 transition-all active:scale-95"
            >
              取消
            </button>
            <button
              type="submit"
              disabled={loading}
              className="flex-1 px-4 py-2 rounded-xl bg-[#2f4dd5] text-white font-semibold hover:bg-blue-600 transition-all active:scale-95 disabled:opacity-50"
            >
              {loading ? '建立中...' : '建立類別'}
            </button>
          </div>
        </form>
      </div>
    </div>,
    document.body
  );


};
