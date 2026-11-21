import { useEffect, useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import axiosClient from '../api/axiosClient';

export default function Admin() {
  const [movies, setMovies] = useState([]);
  const [producers, setProducers] = useState([]);
  const [genres, setGenres] = useState([]);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const load = () => {
    Promise.all([
      axiosClient.get('/Movies'),
      axiosClient.get('/Producers'),
      axiosClient.get('/Genres'),
    ]).then(([m, p, g]) => {
      setMovies(m.data);
      setProducers(p.data);
      setGenres(g.data);
    }).catch((err) => {
      if (err.response?.status === 403) navigate('/');
      setError('Failed to load admin data.');
    });
  };

  useEffect(() => { load(); }, []);

  const handleDelete = async (id) => {
    if (!confirm('Delete this movie?')) return;
    try {
      await axiosClient.delete(`/Movies/${id}`);
      load();
    } catch (err) {
      alert(err.response?.data?.message || 'Delete failed.');
    }
  };

  return (
    <div className="min-h-screen p-8">
      <div className="flex justify-between items-center mb-8">
        <div>
          <Link to="/" className="text-xs uppercase tracking-widest text-gray-500 hover:text-cinema-accent">
            ← Back to wheel
          </Link>
          <h1 className="font-display text-5xl tracking-wider mt-2">ADMIN PANEL</h1>
        </div>
        <div className="flex gap-3">
            <Link to="/admin/genres" className="bg-cinema-gray hover:bg-gray-700 px-6 py-3 rounded-lg text-sm font-semibold">
                Genres
            </Link>
            <Link to="/admin/producers" className="bg-cinema-gray hover:bg-gray-700 px-6 py-3 rounded-lg text-sm font-semibold">
                Producers
            </Link>
            <Link to="/admin/movie/new" className="bg-cinema-accent hover:bg-red-700 px-6 py-3 rounded-lg text-sm font-semibold">
                + New Movie
            </Link>
        </div>
      </div>

      {error && <div className="text-red-400 mb-4">{error}</div>}

      <div className="bg-cinema-dark rounded-2xl border border-white/5 overflow-hidden">
        <table className="w-full text-left">
          <thead className="bg-cinema-gray text-xs uppercase tracking-widest text-gray-400">
            <tr>
              <th className="p-4">Poster</th>
              <th className="p-4">Title</th>
              <th className="p-4">Year</th>
              <th className="p-4">Rating</th>
              <th className="p-4 text-right">Actions</th>
            </tr>
          </thead>
          <tbody>
            {movies.map((m) => (
              <tr key={m.id} className="border-t border-white/5 hover:bg-white/[0.02]">
                <td className="p-4">
                  <img src={m.posterLink} alt={m.title} className="w-12 h-16 object-cover rounded" />
                </td>
                <td className="p-4 font-semibold">{m.title}</td>
                <td className="p-4 text-gray-400">{m.releaseYear}</td>
                <td className="p-4 text-gray-400">{m.rating}</td>
                <td className="p-4 text-right">
                  <Link to={`/admin/movie/${m.id}/edit`} className="text-cinema-accent hover:underline mr-4">Edit</Link>
                  <button onClick={() => handleDelete(m.id)} className="text-red-500 hover:underline">Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}