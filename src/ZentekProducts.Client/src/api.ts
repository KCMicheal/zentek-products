import type { Product, CreateProductDto, LoginDto, TokenDto } from './types';

const API_URL = 'http://localhost:5000';

class ApiService {
  private token: string | null = null;

  setToken(token: string | null) {
    this.token = token;
    if (token) {
      localStorage.setItem('token', token);
    } else {
      localStorage.removeItem('token');
    }
  }

  getToken(): string | null {
    if (!this.token) {
      this.token = localStorage.getItem('token');
    }
    return this.token;
  }

  private async request<T>(endpoint: string, options: RequestInit = {}): Promise<T> {
    const headers: Record<string, string> = {
      'Content-Type': 'application/json',
      ...options.headers as Record<string, string>,
    };

    const token = this.getToken();
    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    const response = await fetch(`${API_URL}${endpoint}`, {
      ...options,
      headers,
    });

    if (!response.ok) {
      const contentType = response.headers.get('content-type') ?? '';
      let details = '';
      try {
        if (contentType.includes('application/json')) {
          const json = (await response.json()) as unknown;
          details = JSON.stringify(json);
        } else {
          details = await response.text();
        }
      } catch {
        // ignore parse errors, fall back to status text
      }

      const msg = details ? `Request failed (${response.status}): ${details}` : `Request failed (${response.status}): ${response.statusText}`;
      throw new Error(msg);
    }

    if (response.status === 204) {
      return null as T;
    }

    return response.json();
  }

  async login(dto: LoginDto): Promise<TokenDto> {
    return this.request<TokenDto>('/api/auth/login', {
      method: 'POST',
      body: JSON.stringify(dto),
    });
  }

  async getProducts(colour?: string): Promise<Product[]> {
    const query = colour ? `?colour=${encodeURIComponent(colour)}` : '';
    return this.request<Product[]>(`/api/products${query}`);
  }

  async createProduct(dto: CreateProductDto): Promise<Product> {
    return this.request<Product>('/api/products', {
      method: 'POST',
      body: JSON.stringify(dto),
    });
  }

  async checkHealth(): Promise<{ status: string }> {
    return this.request<{ status: string }>('/health');
  }
}

export const api = new ApiService();