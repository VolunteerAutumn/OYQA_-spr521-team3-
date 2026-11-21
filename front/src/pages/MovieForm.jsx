import { useEffect, useState } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import axiosClient from '../api/axiosClient';

export default function MovieForm() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();

  const [form, setForm] = useState({
    title: '',
    description: '',
    releaseYear: 2024,
    rating: 5,
    producerId: '',
    posterLink: '',
    genreIds: [],
  });
  const [producers, setProducers] = useState([]);
  const [genres, setGenres] = useState([]);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    Promise.all([axiosClient.get('/Producers'), axiosClient.get('/Genres')])
      .then(([p, g]) => {
        setProducers(p.data);
        setGenres(g.data);
        if (p.data.length && !isEdit) setForm((f) => ({ ...f, producerId: p.data[0].id }));
      });

    if (isEdit) {
      axiosClient.get(`/Movies/${id}`).then((res) => {
        const m = res.data;
        setForm({
          title: m.title,
          description: m.description,
          releaseYear: m.releaseYear,
          rating: m.rating,
          producerId: m.producer?.id ?? m.producerId ?? '',
          posterLink: m.posterLink,
          genreIds: m.genres?.map((g) => g.id) ?? [],
        });
      });
    }
  }, [id, isEdit]);

  const handle = (e) => {
    const { name, value } = e.target;
    setForm({ ...form, [name]: name === 'releaseYear' || name === 'rating' || name === 'producerId' ? Number(value) : value });
  };

  const toggleGenre = (genreId) => {
    setForm((f) => ({
      ...f,
      genreIds: f.genreIds.includes(genreId)
        ? f.genreIds.filter((id) => id !== genreId)
        : [...f.genreIds, genreId],
    }));
  };

  const submit = async (e) => {
    e.preventDefault();
    setError(null);
    setLoading(true);
    try {
      if (isEdit) await axiosClient.put(`/Movies/${id}`, form);
      else await axiosClient.post('/Movies', form);
      navigate('/admin');
    } catch (err) {
      setError(err.response?.data?.message || 'Save failed.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen p-8">
      <Link to="/admin" className="text-xs uppercase tracking-widest text-gray-500 hover:text-cinema-accent">
        ← Back to admin
      </Link>

      <h1 className="font-display text-5xl tracking-wider mt-2 mb-8">
        {isEdit ? 'EDIT MOVIE' : 'NEW MOVIE'}
      </h1>

      <form onSubmit={submit} className="max-w-3xl bg-cinema-dark border border-white/5 rounded-2xl p-8 space-y-5">
        <div>
          <label className="block text-xs uppercase tracking-widest text-gray-400 mb-2">Title</label>
          <input name="title" value={form.title} onChange={handle} required
            className="w-full bg-cinema-gray border border-white/10 rounded-lg px-4 py-3" />
        </div>

        <div>
          <label className="block text-xs uppercase tracking-widest text-gray-400 mb-2">Description</label>
          <textarea name="description" value={form.description} onChange={handle} rows={4}
            className="w-full bg-cinema-gray border border-white/10 rounded-lg px-4 py-3" />
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-xs uppercase tracking-widest text-gray-400 mb-2">Release Year</label>
            <input name="releaseYear" type="number" value={form.releaseYear} onChange={handle} required
              className="w-full bg-cinema-gray border border-white/10 rounded-lg px-4 py-3" />
          </div>
          <div>
            <label className="block text-xs uppercase tracking-widest text-gray-400 mb-2">Rating (0-10)</label>
            <input name="rating" type="number" min="0" max="10" value={form.rating} onChange={handle} required
              className="w-full bg-cinema-gray border border-white/10 rounded-lg px-4 py-3" />
          </div>
        </div>

        <div>
          <label className="block text-xs uppercase tracking-widest text-gray-400 mb-2">Poster URL</label>
          <input name="posterLink" value={form.posterLink} onChange={handle}
            className="w-full bg-cinema-gray border border-white/10 rounded-lg px-4 py-3" />
        </div>

        <div>
          <label className="block text-xs uppercase tracking-widest text-gray-400 mb-2">Producer</label>
          <select name="producerId" value={form.producerId} onChange={handle} required
            className="w-full bg-cinema-gray border border-white/10 rounded-lg px-4 py-3">
            {producers.map((p) => <option key={p.id} value={p.id}>{p.name}</option>)}
          </select>
        </div>

        <div>
          <label className="block text-xs uppercase tracking-widest text-gray-400 mb-2">Genres</label>
          <div className="flex flex-wrap gap-2">
            {genres.map((g) => (
              <button type="button" key={g.id} onClick={() => toggleGenre(g.id)}
                className={`px-4 py-2 rounded-full text-xs uppercase tracking-widest border transition ${
                  form.genreIds.includes(g.id)
                    ? 'bg-cinema-accent border-cinema-accent text-white'
                    : 'bg-cinema-gray border-white/10 text-gray-400'
                }`}>
                {g.name}
              </button>
            ))}
          </div>
        </div>

        {error && <div className="text-red-400 text-sm bg-red-500/10 border border-red-500/20 rounded-lg px-3 py-2">{error}</div>}

        <button type="submit" disabled={loading}
          className="w-full bg-cinema-accent hover:bg-red-700 py-3 rounded-lg font-semibold transition disabled:opacity-50">
          {loading ? 'Saving...' : isEdit ? 'Save changes' : 'Create movie'}
        </button>
      </form>
    </div>
  );
}