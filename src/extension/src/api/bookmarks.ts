import api from './client';
import type { Category, Bookmark } from './types';

export const bookmarkApi = {
  getCategories: async (): Promise<Category[]> => {
    const { data } = await api.get('/categories');
    return data;
  },
  createCategory: async (category: Partial<Category>) => {
    const { data } = await api.post('/categories', category);
    return data;
  },
  updateCategory: async (id: string, category: Partial<Category>) => {
    const { data } = await api.patch(`/categories/${id}`, category);
    return data;
  },
  updateCategoryOrder: async (orders: { categoryId: string, sortOrder: number }[]) => {
    await api.patch('/categories/order', orders);
  },
  updateUnifiedOrder: async (categoryId: string, orders: { itemId: string, type: 'Bookmark' | 'Category', sortOrder: number }[]) => {
    await api.patch(`/categories/unified-order?categoryId=${categoryId}`, orders);
  },
  getUnifiedItems: async (categoryId: string) => {
    const { data } = await api.get(`/categories/unified/${categoryId}`);
    return data;
  },
  deleteCategory: async (id: string) => {
    await api.delete(`/categories/${id}`);
  },
  updateBookmark: async (id: string, bookmark: Partial<Bookmark>) => {
    const { data } = await api.patch(`/bookmarks/${id}`, bookmark);
    return data;
  },
  updateBookmarkOrder: async (bookmarkIds: string[]) => {
    await api.patch('/bookmarks/order', bookmarkIds);
  },
  getBookmarks: async (categoryId?: string): Promise<Bookmark[]> => {
    const { data } = await api.get('/bookmarks', { params: { categoryId } });
    return data;
  },
  createBookmark: async (bookmark: Partial<Bookmark>) => {
    const { data } = await api.post('/bookmarks', bookmark);
    return data;
  },
  deleteBookmark: async (id: string) => {
    await api.delete(`/bookmarks/${id}`);
  },
  importBookmarks: async (bookmarks: any[]) => {
    const { data } = await api.post('/bookmarks/import', bookmarks);
    return data;
  }
};
