import { Routes, Route } from 'react-router-dom';
import Login from './pages/Login';
import Register from './pages/Register';
import Home from './pages/home';
import MovieDetails from './pages/MovieDetails';
import Admin from './pages/Admin';
import MovieForm from './pages/MovieForm';
import AdminEntities from './pages/AdminEntities';

function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      <Route path="/" element={<Home />} />
      <Route path="/movies/:id" element={<MovieDetails />} />
      <Route path="/admin" element={<Admin />} />
      <Route path="/admin/movie/new" element={<MovieForm />} />
      <Route path="/admin/movie/:id/edit" element={<MovieForm />} />
      <Route path="/admin/:type" element={<AdminEntities />} />
    </Routes>
  );
}

export default App;