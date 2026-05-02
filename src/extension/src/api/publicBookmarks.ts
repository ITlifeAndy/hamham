import api from './client';

export interface PublicBookmark {
  id: string;
  name: string;
  url: string;
  category: string;
}

export interface SharePool {
  id: string;
  name: string;
}

export const publicBookmarkApi = {
  async getBookmarks(poolId?: string) {
    const params = poolId ? { poolId } : {};
    const response = await api.get('/public-bookmarks', { params });
    return response.data;
  },
  async getPools() {
    const response = await api.get('/share-pools');
    return response.data;
  },
  async createPool(name: string) {
    const response = await api.post('/share-pools', { name });
    return response.data;
  },
  async updatePool(id: string, name: string) {
    const response = await api.put(`/share-pools/${id}`, { name });
    return response.data;
  },
  async deletePool(id: string) {
    await api.delete(`/share-pools/${id}`);
  },
  async createBookmark(poolId: string, name: string, url: string) {
    const response = await api.post('/share-pool-bookmarks', { poolId, name, url });
    return response.data;
  },
  async updateBookmark(id: string, name: string, url: string) {
    const response = await api.put(`/share-pool-bookmarks/${id}`, { name, url });
    return response.data;
  },
  async deleteBookmark(id: string) {
    await api.delete(`/share-pool-bookmarks/${id}`);
  }
};
