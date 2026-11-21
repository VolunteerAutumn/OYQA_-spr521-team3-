import { useEffect, useState } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import axiosClient from '../api/axiosClient';

export default function AdminEntities() {
  const { type } = useParams();
  const isGenre = type?.toLowerCase() === 'genres';
  const title = isGenre ? 'GENRES' : 'PRODUCERS';
  const endpoint = isGenre ? '/Genres' : '/Producers';
  const dtoKey = isGenre ? 'genre' : 'producer';

  const [items, setItems] = useState([]);
  const [form, setForm] = useState({ id: null, name: '' });
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const load = () => {
    setError(null);
    axiosClient.get(endpoint)
      .then((res) => setItems(res.data))
      .catch((err) => {
        if (err.response?.status === 403) navigate('/');
        else setError('Failed to load ' + dtoKey + 's.');
      });
  };

  useEffect(() => {
    setForm({ id: null, name: '' });
    load();
  }, [type]);

  const submit = async (e) => {
    e.preventDefault();
    setError(null);
    try {
      if (form.id) {
        await axiosClient.put(`${endpoint}/${form.id}`, { name: form.name });
      } else {
        await axiosClient.post(endpoint, { name: form.name });
      }
      setForm({ id: null, name: '' });
      load();
    } catch (err) {
      setError(err.response?.data?.message || 'Save failed.');
    }
  };

  const handleDelete = async (id) => {
    if (!confirm('Delete this item?')) return;
    try {
      await axiosClient.delete(`${endpoint}/${id}`);
      load();
    } catch (err) {
      alert(err.response?.data?.message || 'Delete failed (probably referenced by a movie).');
    }
  };

  const edit = (item) => setForm({ id: item.id, name: item.name });

  return (
    <div className="min-h-screen p-8">
      <Link to="/admin" className="text-xs uppercase tracking-widest text-gray-500 hover:text-cinema-accent">
        ← Back to admin
      </Link>

      <h1 className="font-display text-5xl tracking-wider mt-2 mb-8">{title}</h1>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
        {/* Form */}
        <form onSubmit={submit} className="bg-cinema-dark border border-white/5 rounded-2xl p-6 h-fit">
          <h2 className="text-xl font-semibold mb-4">
            {form.id ? 'Edit' : 'Add new'} {dtoKey}
          </h2>

          <label className="block text-xs uppercase tracking-widest text-gray-400 mb-2">Name</label>
          <input
            value={form.name}
            onChange={(e) => setForm({ ...form, name: e.target.value })}
            required
            className="w-full bg-cinema-gray border border-white/10 rounded-lg px-4 py-3 mb-4"
          />

          {error && <div className="text-red-400 text-sm mb-4">{error}</div>}

          <div className="flex gap-3">
            <button type="submit" className="bg-cinema-accent hover:bg-red-700 px-6 py-3 rounded-lg font-semibold flex-1">
              {form.id ? 'Save changes' : 'Create'}
            </button>
            {form.id && (
              <button type="button" onClick={() => setForm({ id: null, name: '' })}
                className="bg-cinema-gray px-6 py-3 rounded-lg text-gray-400">
                Cancel
              </button>
            )}
          </div>
        </form>

        {/* List */}
        <div className="bg-cinema-dark border border-white/5 rounded-2xl overflow-hidden">
          <table className="w-full text-left">
            <thead className="bg-cinema-gray text-xs uppercase tracking-widest text-gray-400">
              <tr>
                <th className="p-4">ID</th>
                <th className="p-4">Name</th>
                <th className="p-4 text-right">Actions</th>
              </tr>
            </thead>
            <tbody>
              {items.map((item) => (
                <tr key={item.id} className="border-t border-white/5 hover:bg-white/[0.02]">
                  <td className="p-4 text-gray-500">{item.id}</td>
                  <td className="p-4">{item.name}</td>
                  <td className="p-4 text-right">
                    <button onClick={() => edit(item)} className="text-cinema-accent hover:underline mr-4">Edit</button>
                    <button onClick={() => handleDelete(item.id)} className="text-red-500 hover:underline">Delete</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}