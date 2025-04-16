import axios from 'axios';
import { Book } from '../types/book';
import { SearchBooksParams } from '../types/searchBookParams';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const searchBooks = async (
  params: SearchBooksParams
): Promise<Book[]> => {
  try {
    const response = await api.get(`/books/search`, { params });
    return response.data;
  } catch (error) {
    console.error('Error fetching books:', error);
    throw error;
  }
};
