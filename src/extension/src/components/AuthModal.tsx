import React, { useState, useEffect } from 'react';
import { authApi } from '../api/auth';
import { storage } from '../services/storage';

interface AuthModalProps {
  isOpen: boolean;
  onClose: () => void;
  onAuthSuccess: () => void;
}

export const AuthModal: React.FC<AuthModalProps> = ({ isOpen, onClose, onAuthSuccess }) => {
  const [isLogin, setIsLogin] = useState(true);
  const [form, setForm] = useState({ name: '', username: '', email: '', password: '', hostUrl: '' });

  useEffect(() => {
    if (isOpen) {
      storage.get('api_host_url').then(url => {
        if (url) setForm(prev => ({ ...prev, hostUrl: url }));
      });
    }
  }, [isOpen]);

  if (!isOpen) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!form.hostUrl.trim()) {
      alert('請填寫主機 URL');
      return;
    }

    if (!/^https?:\/\//.test(form.hostUrl)) {
      alert('請輸入正確的主機 URL (必須以 http:// 或 https:// 開頭)');
      return;
    }

    if (isLogin) {
      if (!form.username.trim() || !form.password.trim()) {
        alert('請填寫帳號與密碼');
        return;
      }
    } else {
      if (!form.name.trim() || !form.username.trim() || !form.email.trim() || !form.password.trim()) {
        alert('請填寫所有必要欄位');
        return;
      }
    }

    try {
      if (isLogin) {
        const res = await authApi.login(form.username, form.password, form.hostUrl);
        localStorage.setItem('hamham_token', res.token);
        await storage.set('api_host_url', form.hostUrl);
      } else {
        await authApi.register(form.name, form.username, form.email, form.password, form.hostUrl);
      }
      onAuthSuccess();
      onClose();
    } catch (err) {
      alert('操作失敗，請檢查主機 URL 或帳密');
    }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm">
      <div className="bg-white rounded-hamham p-8 w-full max-w-md shadow-2xl">
        <div className="flex justify-center mb-6">
          <img src="/logob.png" alt="HamHam Logo" className="w-20 h-20 object-contain" />
        </div>
        <h2 className="text-2xl font-bold mb-6 text-center">
          {isLogin ? 'Welcome Back' : 'Create Account'}
        </h2>
          <form onSubmit={handleSubmit} className="space-y-4">
            <input
              type="text"
              placeholder="主機 URL (e.g. https://api.hamham.com)"
              className="w-full p-3 border rounded-xl"
              value={form.hostUrl}
              onChange={e => setForm({...form, hostUrl: e.target.value})}
              required
            />
            {!isLogin && (
              <>
                <input
                  type="text"
                  placeholder="姓名"
                  className="w-full p-3 border rounded-xl"
                  onChange={e => setForm({...form, name: e.target.value})}
                  required
                />
                <input
                  type="email"
                  placeholder="Email"
                  className="w-full p-3 border rounded-xl"
                  onChange={e => setForm({...form, email: e.target.value})}
                  required
                />
              </>
            )}
            <input
              type="text"
              placeholder="Username"
              className="w-full p-3 border rounded-xl"
              onChange={e => setForm({...form, username: e.target.value})}
              required
            />
            <input
              type="password"
              placeholder="Password"
              className="w-full p-3 border rounded-xl"
              onChange={e => setForm({...form, password: e.target.value})}
              required
            />
            <button type="submit" className="w-full py-3 bg-blue-600 text-white font-bold rounded-xl hover:bg-blue-700 transition-all">
              {isLogin ? 'Login' : 'Register'}
            </button>
          </form>
         <button 
           onClick={() => setIsLogin(!isLogin)} 
           className="w-full mt-4 text-sm text-slate-500 hover:underline"
         >
           {/* {isLogin ? 'Need an account? Register' : 'Already have an account? Login'} */}
         </button>
       </div>
     </div>
   );
};

