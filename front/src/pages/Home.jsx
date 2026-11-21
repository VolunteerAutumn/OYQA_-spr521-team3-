
import { useEffect, useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import axiosClient from '../api/axiosClient';
import MovieWheel from '../components/MovieWheel';

export default function Home() {
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const navigate = useNavigate();

// At the top of Home component, after `const navigate = useNavigate();`
const token = localStorage.getItem('token');
let isAdmin = false;
if (token) {
  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    isAdmin = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] === 'Admin'
           || (Array.isArray(payload.role) && payload.role.includes('Admin'))
           || payload.role === 'Admin';
  } catch { /* ignore */ }
}

  const fetchMovies = useCallback(async () => {
    setLoading(true);
    setError('');

    try {
      const token = localStorage.getItem('token');

      if (!token) {
        navigate('/login');
        return;
      }

      const res = await axiosClient.get('/Movies');
      setMovies(Array.isArray(res.data) ? res.data : []);
    } catch (err) {
      console.error('Failed to load movies:', err);

      if (err.response?.status === 401) {
        localStorage.removeItem('token');
        navigate('/login');
        return;
      }

      setError(
        err.response?.data?.message ||
        'Something went wrong while loading your movie collection.'
      );
    } finally {
      setLoading(false);
    }
  }, [navigate]);

  useEffect(() => {
    fetchMovies();
  }, [fetchMovies]);

  const logout = () => {
    localStorage.removeItem('token');
    navigate('/login');
  };

  const handleSelectMovie = (movie) => {
    navigate(`/movies/${movie.id}`);
  };

  return (
    <main className="relative min-h-screen overflow-hidden bg-[#080808] text-white">

      {/* Background atmosphere */}
      <div className="pointer-events-none fixed inset-0">
        <div className="absolute -top-64 left-1/2 h-[600px] w-[600px] -translate-x-1/2 rounded-full bg-red-700/[0.04] blur-[140px]" />
        <div className="absolute inset-0 bg-[radial-gradient(ellipse_at_center,transparent_20%,#080808_100%)]" />
      </div>

      {/* Header */}
      <header className="relative z-30 border-b border-white/[0.06]">
        <div className="mx-auto flex max-w-7xl items-center justify-between px-5 py-5 sm:px-10">

          <div className="flex items-center gap-3">
            <div className="flex h-10 w-10 items-center justify-center rounded-lg border border-red-500/30 bg-red-500/10">
              <span className="text-lg">🎬</span>
            </div>

            <div>
              <h1 className="font-display text-xl font-bold tracking-[0.2em] sm:text-2xl">
                MOVIE<span className="text-red-500">WHEEL</span>
              </h1>
              <p className="text-[8px] uppercase tracking-[0.35em] text-gray-600 sm:text-[10px]">
                Your personal cinema
              </p>
            </div>
          </div>

          <button
            onClick={logout}
            className="rounded-lg border border-white/10 bg-white/[0.03] px-4 py-2 text-xs font-medium text-gray-400 transition hover:border-red-500/30 hover:bg-red-500/10 hover:text-red-400"
          >
            Logout <span className="ml-1">↗</span>
          </button>
          {isAdmin && (
            <button
              onClick={() => navigate('/admin')}
              className="rounded-lg bg-red-600/20 border border-red-500/40 px-4 py-2 text-xs font-medium text-red-300 hover:bg-red-600/30"
            >
              Admin Panel
            </button>
          )}
        </div>
      </header>

      {/* Content */}
      <div className="relative z-10 mx-auto max-w-7xl px-4 sm:px-8">

        <div className="pt-12 text-center sm:pt-16">
          <p className="text-[10px] uppercase tracking-[0.5em] text-gray-600">
            Discover something worth watching
          </p>

          <h2 className="mt-3 font-display text-3xl font-bold tracking-tight sm:text-5xl">
            PICK YOUR <span className="text-red-500">NEXT MOVIE.</span>
          </h2>

          <p className="mx-auto mt-4 max-w-md text-sm leading-6 text-gray-500">
            Spin through your collection. Find your next obsession.
          </p>
        </div>

        {loading ? (
          <div className="flex min-h-[50vh] flex-col items-center justify-center">
            <div className="h-10 w-10 animate-spin rounded-full border-2 border-white/10 border-t-red-500" />
            <p className="mt-5 text-[10px] uppercase tracking-[0.35em] text-gray-500">
              Loading your collection...
            </p>
          </div>
        ) : error ? (
          <div className="flex min-h-[45vh] items-center justify-center">
            <div className="max-w-md rounded-2xl border border-red-500/10 bg-white/[0.02] p-8 text-center">
              <div className="text-4xl">⚠️</div>

              <h3 className="mt-4 text-lg font-bold text-white">
                THE PROJECTOR IS BROKEN
              </h3>

              <p className="mt-3 text-sm leading-6 text-gray-500">
                {error}
              </p>

              <button
                onClick={fetchMovies}
                className="mt-6 rounded-lg bg-red-600 px-6 py-3 text-xs font-bold uppercase tracking-widest transition hover:bg-red-500"
              >
                Try again
              </button>
            </div>
          </div>
        ) : (
          <>
            <MovieWheel
              movies={movies}
              onSelect={handleSelectMovie}
            />

            <div className="pb-8 text-center">
              <p className="text-[10px] uppercase tracking-[0.3em] text-gray-700">
                {movies.length} {movies.length === 1 ? 'movie' : 'movies'} in your collection
              </p>
            </div>
          </>
        )}
      </div>

      {/* Footer */}
      <footer className="relative z-10 border-t border-white/[0.04] px-6 py-5 text-center">
        <p className="text-[9px] uppercase tracking-[0.4em] text-gray-700">
          MOVIEWHEEL <span className="text-red-900">©</span> YOUR PERSONAL CINEMA
        </p>
      </footer>
    </main>
  );
}
