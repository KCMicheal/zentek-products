export interface Product {
  id: number;
  name: string;
  description: string | null;
  price: number;
  colour: string;
  category: string | null;
  stockQty: number;
  isActive: boolean;
  createdAt: string;
}

export interface CreateProductDto {
  name: string;
  description?: string;
  price: number;
  colour: string;
  category?: string;
  stockQty: number;
  isActive: boolean;
}

export interface LoginDto {
  username: string;
  password: string;
}

export interface TokenDto {
  token: string;
  expires: string;
}