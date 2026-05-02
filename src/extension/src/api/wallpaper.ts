import api from './client';

export const wallpaperApi = {
  getCurrent: async () => {
    const { data } = await api.get('/wallpaper/current');
    return data.url;
  },
  refresh: async () => {
    const { data } = await api.post('/wallpaper/refresh');
    return data.url;
  },
  updatePreferences: async (prefs: any) => {
    await api.patch('/wallpaper/preferences', prefs);
  },
  getPreferences: async () => {
    const { data } = await api.get('/wallpaper/preferences');
    return data;
  },
  upload: async (formData: FormData) => {
    const { data } = await api.post('/wallpaper/upload', formData);
    return data.path || data.Path;
  }
};
