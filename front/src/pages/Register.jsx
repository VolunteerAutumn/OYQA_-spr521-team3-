import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import axiosClient from '../api/axiosClient';

export default function Register() {
  const [form, setForm] = useState({
    username: '',
    email: '',
    password: '',
    dateOfBirth: '',
    isSubscribedToNewsletter: false,
  });
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handle = (e) => {
    const { name, value, type, checked } = e.target;
    setForm({ ...form, [name]: type === 'checkbox' ? checked : value });
  };

  const submit = async (e) => {
    e.preventDefault();
    setError(null);
    setLoading(true);
    try {
      await axiosClient.post('/Auth/register', {
        ...form,
        dateOfBirth: form.dateOfBirth ? new Date(form.dateOfBirth).toISOString() : null,
      });
      navigate('/login');
    } catch (err) {
      setError(err.response?.data?.message || 'Registration failed.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center px-4 relative overflow-hidden">
      <div className="absolute -top-40 -left-40 w-96 h-96 bg-cinema-accent rounded-full blur-[120px] opacity-30"></div>
      <div className="absolute -bottom-40 -right-40 w-96 h-96 bg-cinema-accent rounded-full blur-[120px] opacity-20"></div>

      <div className="relative z-10 w-full max-w-md">
        <Link to="/login" className="text-xs uppercase tracking-widest text-gray-500 hover:text-cinema-accent">
          ← Back to login
        </Link>
        <h1 className="font-display text-7xl text-center tracking-wider mt-4 mb-8">MOVIEWHEEL</h1>

        <form onSubmit={submit} className="bg-cinema-dark border border-white/5 rounded-2xl p-8 shadow-2xl">
          <h2 className="text-2xl font-semibold mb-6">Create account</h2>

          {['username', 'email', 'password'].map((field) => (
            <div key={field} className="mb-4">
              <label className="block text-xs uppercase tracking-widest text-gray-400 mb-2">{field}</label>
              <input
                name={field}
                type={field === 'password' ? 'password' : field === 'email' ? 'email' : 'text'}
                value={form[field]}
                onChange={handle}
                required
                className="w-full bg-cinema-gray border border-white/10 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-cinema-accent transition"
              />
            </div>
          ))}

          <div className="mb-4">
            <label className="block text-xs uppercase tracking-widest text-gray-400 mb-2">Date of birth</label>
            <input
              name="dateOfBirth"
              type="date"
              value={form.dateOfBirth}
              onChange={handle}
              className="w-full bg-cinema-gray border border-white/10 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-cinema-accent transition"
            />
          </div>

          <label className="flex items-center gap-2 mb-6 text-sm text-gray-400">
            <input
              name="isSubscribedToNewsletter"
              type="checkbox"
              checked={form.isSubscribedToNewsletter}
              onChange={handle}
              className="accent-cinema-accent"
            />
            Subscribe to newsletter
          </label>

          {error && <div className="mb-4 text-red-400 text-sm bg-red-500/10 border border-red-500/20 rounded-lg px-3 py-2">{error}</div>}

          <button
            type="submit"
            disabled={loading}
            className="w-full bg-cinema-accent hover:bg-red-700 text-white font-semibold py-3 rounded-lg transition disabled:opacity-50"
          >
            {loading ? 'Creating account...' : 'Register'}
          </button>
        </form>
      </div>
    </div>
  );
}