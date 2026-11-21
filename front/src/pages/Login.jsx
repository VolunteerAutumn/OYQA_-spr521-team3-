import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axiosClient from '../api/axiosClient';

export default function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setError(null);
    setLoading(true);
    try {
      const res = await axiosClient.post('/Auth/login', { email, password });
      localStorage.setItem('token', res.data.token);
      navigate('/');
    } catch (err) {
      setError(err.response?.data?.message || 'Login failed. Check your credentials.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center px-4 relative overflow-hidden">
      {/* Decorative gradient blob */}
      <div className="absolute -top-40 -left-40 w-96 h-96 bg-cinema-accent rounded-full blur-[120px] opacity-30"></div>
      <div className="absolute -bottom-40 -right-40 w-96 h-96 bg-cinema-accent rounded-full blur-[120px] opacity-20"></div>

      <div className="relative z-10 w-full max-w-md">
        <h1 className="font-display text-7xl text-center text-white tracking-wider mb-2">
          MOVIEWHEEL
        </h1>
        <p className="text-center text-gray-400 mb-10 text-sm tracking-widest uppercase">
          Your cinema, spinning
        </p>

        <form onSubmit={handleLogin} className="bg-cinema-dark border border-white/5 rounded-2xl p-8 shadow-2xl">
          <h2 className="text-2xl font-semibold mb-6 text-white">Sign in</h2>
          
          <div className="mb-4">
            <label className="block text-xs uppercase tracking-widest text-gray-400 mb-2">Email</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              className="w-full bg-cinema-gray border border-white/10 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-cinema-accent transition"
              placeholder="you@example.com"
            />
          </div>

          <div className="mb-6">
            <label className="block text-xs uppercase tracking-widest text-gray-400 mb-2">Password</label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              className="w-full bg-cinema-gray border border-white/10 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-cinema-accent transition"
              placeholder="••••••••"
            />
          </div>

          {error && (
            <div className="mb-4 text-red-400 text-sm bg-red-500/10 border border-red-500/20 rounded-lg px-3 py-2">
              {error}
            </div>
          )}

          <button
            type="submit"
            disabled={loading}
            className="w-full bg-cinema-accent hover:bg-red-700 text-white font-semibold py-3 rounded-lg transition disabled:opacity-50"
          >
            {loading ? 'Signing in...' : 'Sign in'}
          </button>

          <p className="text-center text-gray-500 text-sm mt-6">
            No account? <a href="/register" className="text-cinema-accent hover:underline">Register</a>
          </p>
        </form>
      </div>
    </div>
  );
}