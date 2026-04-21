import { useCallback, useEffect, useState } from 'react';
import type { Product, CreateProductDto } from '../types';
import { api } from '../api';

export function Products() {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [colourFilter, setColourFilter] = useState('');
  const [showForm, setShowForm] = useState(false);
  const [formData, setFormData] = useState<Omit<CreateProductDto, 'price' | 'stockQty'> & { price: string; stockQty: string }>({
    name: '',
    description: '',
    price: '',
    colour: '',
    category: '',
    stockQty: '',
    isActive: true,
  });

  const fetchProducts = useCallback(async () => {
    try {
      const data = await api.getProducts(colourFilter || undefined);
      setProducts(data);
    } catch {
      setError('Failed to load products');
    } finally {
      setLoading(false);
    }
  }, [colourFilter]);

  useEffect(() => {
    const id = window.setTimeout(() => {
      void fetchProducts();
    }, 0);
    return () => window.clearTimeout(id);
  }, [fetchProducts]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const dto: CreateProductDto = {
        ...formData,
        price: Number(formData.price),
        stockQty: Number(formData.stockQty),
      };
      await api.createProduct(dto);
      setShowForm(false);
      setFormData({
        name: '',
        description: '',
        price: '',
        colour: '',
        category: '',
        stockQty: '',
        isActive: true,
      });
      setLoading(true);
      fetchProducts();
    } catch {
      setError('Failed to create product');
    }
  };

  const handleLogout = () => {
    api.setToken(null);
    window.location.reload();
  };

  return (
    <div className="products-page">
      <header>
        <h1>Zentek Products</h1>
        <button onClick={handleLogout}>Logout</button>
      </header>

      <div className="controls">
        <input
          type="text"
          placeholder="Filter by colour..."
          value={colourFilter}
          onChange={(e) => {
            setLoading(true);
            setColourFilter(e.target.value);
          }}
        />
        <button onClick={() => setShowForm(!showForm)}>
          {showForm ? 'Cancel' : 'Add Product'}
        </button>
      </div>

      {showForm && (
        <form className="product-form" onSubmit={handleSubmit}>
          <input
            type="text"
            placeholder="Name"
            value={formData.name}
            onChange={(e) => setFormData({ ...formData, name: e.target.value })}
            required
          />
          <input
            type="text"
            placeholder="Description"
            value={formData.description || ''}
            onChange={(e) => setFormData({ ...formData, description: e.target.value })}
          />
          <input
            type="number"
            placeholder="Price"
            value={formData.price}
            onChange={(e) => setFormData({ ...formData, price: e.target.value })}
            required
          />
          <input
            type="text"
            placeholder="Colour"
            value={formData.colour}
            onChange={(e) => setFormData({ ...formData, colour: e.target.value })}
            required
          />
          <input
            type="text"
            placeholder="Category"
            value={formData.category || ''}
            onChange={(e) => setFormData({ ...formData, category: e.target.value })}
          />
          <input
            type="number"
            placeholder="Stock Qty"
            value={formData.stockQty}
            onChange={(e) => setFormData({ ...formData, stockQty: e.target.value })}
            required
          />
          <button type="submit">Create</button>
        </form>
      )}

      {error && <div className="error">{error}</div>}

      {loading ? (
        <p>Loading...</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Name</th>
              <th>Description</th>
              <th>Price</th>
              <th>Colour</th>
              <th>Category</th>
              <th>Stock</th>
            </tr>
          </thead>
          <tbody>
            {products.map((p) => (
              <tr key={p.id}>
                <td>{p.id}</td>
                <td>{p.name}</td>
                <td>{p.description}</td>
                <td>${p.price.toFixed(2)}</td>
                <td>{p.colour}</td>
                <td>{p.category}</td>
                <td>{p.stockQty}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}