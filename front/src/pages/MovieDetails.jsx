import { useEffect, useState } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import axiosClient from '../api/axiosClient';

export default function MovieDetails() {
  const { id } = useParams();
  const [movie, setMovie] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    axiosClient.get(`/Movies/${id}`)
      .then((res) => setMovie(res.data))
      .catch(() => navigate('/'))
      .finally(() => setLoading(false));
  }, [id, navigate]);

  if (loading) return <div className="min-h-screen flex items-center justify-center text-gray-500">Loading...</div>;
  if (!movie) return null;

  return (
    <div className="min-h-screen p-8">
      <Link to="/" className="text-xs uppercase tracking-widest text-gray-500 hover:text-cinema-accent">
        ← Back to wheel
      </Link>

      <div className="mt-10 max-w-6xl mx-auto grid grid-cols-1 md:grid-cols-3 gap-10">
        <div className="md:col-span-1">
          <img src={movie.posterLink} alt={movie.title} className="w-full rounded-2xl shadow-2xl border border-white/10" />
        </div>
        <div className="md:col-span-2">
          <h1 className="font-display text-6xl tracking-wider">{movie.title}</h1>
          <div className="mt-3 flex items-center gap-4 text-gray-400">
            <span>{movie.releaseYear}</span>
            <span>⭐ {movie.rating}</span>
            {movie.producer && <span>Produced by {movie.producer.name}</span>}
          </div>
          <p className="mt-6 text-gray-300 leading-relaxed">{movie.description}</p>

          {movie.genres?.length > 0 && (
            <div className="mt-6 flex flex-wrap gap-2">
              {movie.genres.map((g) => (
                <span key={g.id} className="bg-cinema-gray px-3 py-1 rounded-full text-xs uppercase tracking-widest text-gray-300">
                  {g.name}
                </span>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}