import type { AuthResponse } from './types';
import api from './client';
import { storage } from '../services/storage';

export const authApi = {
  login: async (usernameOrEmail: string, password: string, hostUrl?: string): Promise<AuthResponse> => {
    const config = hostUrl ? { baseURL: hostUrl.endsWith('/api') ? hostUrl : `${hostUrl.replace(/\/$/, '')}/api` } : {};
    const { data } = await api.post('/auth/login', { usernameOrEmail, password }, config);
    await storage.set('hamham_token', data.token);
    await storage.set('hamham_user_name', data.name);
    return data;
  },
  register: async (name: string, username: string, email: string, password: string, hostUrl?: string) => {
    const config = hostUrl ? { baseURL: hostUrl.endsWith('/api') ? hostUrl : `${hostUrl.replace(/\/$/, '')}/api` } : {};
    await api.post('/auth/register', { name, username, email, password }, config);
  },
  logout: async () => {
    await storage.remove('hamham_token');
    await storage.remove('hamham_refresh_token');
    await storage.remove('hamham_user_name');
  }
};
