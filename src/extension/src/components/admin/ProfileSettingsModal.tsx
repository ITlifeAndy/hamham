import React, { useState, useEffect } from 'react';
import api, { resolveAvatarUrl } from '../../api/client';
import { useHostUrl } from '../../hooks/useHostUrl';

interface ProfileSettingsModalProps {
  isOpen: boolean;
  onClose: () => void;
  onUserUpdated?: () => void;
  user: {
    username: string;
    name: string;
    email: string;
    avatar?: string;
  } | null;
}

export const ProfileSettingsModal: React.FC<ProfileSettingsModalProps> = ({ isOpen, onClose, onUserUpdated, user }) => {
  const hostUrl = useHostUrl();
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    password: '',
  });
  const [avatarUrl, setAvatarUrl] = useState('');
  const [loading, setLoading] = useState(false);
  const [uploading, setUploading] = useState(false);
  const [message, setMessage] = useState('');

  useEffect(() => {
    if (isOpen) {
      fetchProfile();
    }
  }, [isOpen]);

  const fetchProfile = async () => {
    setLoading(true);
    try {
      const { data } = await api.get('/users/profile');
      setFormData({
        name: data.name || '',
        email: data.email || '',
        password: '',
      });
      setAvatarUrl(data.avatar || '');
    } catch (err: any) {
      setMessage('Failed to load profile data.');
    } finally {
      setLoading(false);
    }
  };

  if (!isOpen) return null;

  const handleAvatarUpload = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;

    // Validation: File size (2MB limit)
    if (file.size > 2 * 1024 * 1024) {
      setMessage('檔案大小不能超過 2MB');
      return;
    }

    // Validation: Aspect Ratio (1:1)
    const img = new Image();
    img.src = URL.createObjectURL(file);
    await new Promise((resolve) => (img.onload = resolve));
    URL.revokeObjectURL(img.src);

    if (img.width !== img.height) {
      setMessage('請上傳正方形 (1:1) 的圖片');
      return;
    }

    setUploading(true);
    setMessage('');
    const formData = new FormData();
    formData.append('file', file);

    try {
      const { data } = await api.post('/users/avatar', formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
      });
      setAvatarUrl(data.avatarUrl);
      setMessage('Avatar uploaded successfully!');
    } catch (err: any) {
      setMessage(err.response?.data?.message || 'An error occurred during upload.');
    } finally {
      setUploading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setMessage('');
    try {
      await api.put('/users/profile', {
        name: formData.name,
        email: formData.email,
        password: formData.password || undefined,
        avatarUrl: avatarUrl,
      });
      setMessage('Profile updated successfully!');
      if (onUserUpdated) onUserUpdated();
      setTimeout(() => {
        setMessage('');
        onClose();
      }, 2000);
    } catch (err: any) {
      setMessage(err.response?.data?.message || 'An error occurred.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 z-[100] flex items-center justify-center bg-black/50 backdrop-blur-sm animate-in fade-in duration-200">
      <div className="bg-white rounded-3xl shadow-2xl w-full max-w-md p-8 animate-in zoom-in-95 duration-200">
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-2xl font-bold text-slate-900">個人設定</h2>
          <button onClick={onClose} className="text-slate-400 hover:text-slate-600 transition-colors">
            <span className="material-symbols-outlined">close</span>
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="flex flex-col items-center gap-2 mb-6">
            <div className="h-24 w-24 rounded-full bg-slate-100 overflow-hidden border-2 border-slate-200">
               {avatarUrl ? (
                 <img src={resolveAvatarUrl(avatarUrl, hostUrl)} alt="Avatar" className="w-full h-full object-cover" />
               ) : (

                <div className="w-full h-full flex items-center justify-center text-slate-400">
                  <span className="material-symbols-outlined text-4xl">person</span>
                </div>
              )}
            </div>
            <div className="flex flex-col items-center gap-1">
              <label className="cursor-pointer bg-slate-100 hover:bg-slate-200 text-slate-600 px-4 py-1.5 rounded-full text-xs font-semibold transition-colors">
                {uploading ? '上傳中...' : '更改頭像'}
                <input type="file" className="hidden" accept="image/*" onChange={handleAvatarUpload} disabled={uploading} />
              </label>
              <span className="text-xs text-slate-500">建議：正方形 (1:1), 2MB 以下</span>
            </div>
          </div>

          <div>
            <label className="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-1">帳號 (唯讀)</label>
            <input 
              className="w-full px-4 py-2 rounded-lg border border-slate-200 bg-slate-50 text-slate-500 cursor-not-allowed outline-none" 
              value={user?.username || ''} 
              readOnly 
            />
          </div>
          <div>
            <label className="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-1">名稱</label>
            <input 
              required
              className="w-full px-4 py-2 rounded-lg border border-slate-200 focus:ring-2 focus:ring-primary outline-none"
              value={formData.name}
              onChange={e => setFormData({...formData, name: e.target.value})}
            />
          </div>
          <div>
            <label className="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-1">Email</label>
            <input 
              required
              type="email"
              className="w-full px-4 py-2 rounded-lg border border-slate-200 focus:ring-2 focus:ring-primary outline-none"
              value={formData.email}
              onChange={e => setFormData({...formData, email: e.target.value})}
            />
          </div>
          <div>
            <label className="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-1">新密碼 (留空則不修改)</label>
            <input 
              type="password"
              className="w-full px-4 py-2 rounded-lg border border-slate-200 focus:ring-2 focus:ring-primary outline-none"
              value={formData.password}
              onChange={e => setFormData({...formData, password: e.target.value})}
            />
          </div>

          {message && (
            <div className={`p-3 rounded-lg text-sm font-medium ${message.includes('successfully') ? 'bg-green-50 text-green-700' : 'bg-red-50 text-red-700'}`}>
              {message}
            </div>
          )}

          <div className="flex justify-end gap-3 mt-8">
            <button type="button" onClick={onClose} className="px-4 py-2 text-sm font-semibold text-slate-600 hover:bg-slate-50 rounded-lg transition-colors">取消</button>
            <button 
              type="submit" 
              disabled={loading}
              className="px-4 py-2 text-sm font-semibold text-white bg-blue-600 hover:bg-blue-700 rounded-lg transition-colors disabled:opacity-50"
            >
              {loading ? '儲存中...' : '儲存變更'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
