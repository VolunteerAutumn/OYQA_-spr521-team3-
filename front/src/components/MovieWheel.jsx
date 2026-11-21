
import { useState, useEffect, useCallback, useMemo } from 'react';

export default function MovieWheel({ movies = [], onSelect }) {
  const [centerIndex, setCenterIndex] = useState(0);

  const visibleCount = 5;
  const offset = Math.floor(visibleCount / 2);

  const radius = 520;
  const maxAngle = 20;

  const safeMovies = useMemo(() => movies.filter(Boolean), [movies]);

  const next = useCallback(() => {
    if (!safeMovies.length) return;
    setCenterIndex((i) => (i + 1) % safeMovies.length);
  }, [safeMovies.length]);

  const prev = useCallback(() => {
    if (!safeMovies.length) return;
    setCenterIndex((i) => (i - 1 + safeMovies.length) % safeMovies.length);
  }, [safeMovies.length]);

  const selectMovie = useCallback(
    (movie) => {
      if (movie && onSelect) onSelect(movie);
    },
    [onSelect]
  );

  useEffect(() => {
    if (centerIndex >= safeMovies.length) {
      setCenterIndex(0);
    }
  }, [safeMovies.length, centerIndex]);

  useEffect(() => {
    const handleKeyDown = (e) => {
      const target = e.target;

      const isTyping =
        target instanceof HTMLElement &&
        (target.isContentEditable ||
          ['INPUT', 'TEXTAREA', 'SELECT'].includes(target.tagName));

      if (isTyping) return;

      if (e.key === 'ArrowRight') {
        e.preventDefault();
        next();
      }

      if (e.key === 'ArrowLeft') {
        e.preventDefault();
        prev();
      }

      if (e.key === 'Enter') {
        selectMovie(safeMovies[centerIndex]);
      }

      if (e.key === 'Escape') {
        setCenterIndex(0);
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [next, prev, selectMovie, safeMovies, centerIndex]);

  if (!safeMovies.length) {
    return (
      <div className="relative flex min-h-[65vh] items-center justify-center overflow-hidden px-6">
        <div className="absolute h-96 w-96 rounded-full bg-red-600/10 blur-[120px]" />

        <div className="relative text-center">
          <div className="mb-5 text-6xl">🎞️</div>
          <h2 className="font-display text-3xl tracking-wider text-white">
            THE COLLECTION IS EMPTY
          </h2>
          <p className="mt-3 text-sm text-gray-500">
            Your movie universe is waiting to be filled.
          </p>
        </div>
      </div>
    );
  }

  const visible = [];

  for (let position = -offset; position <= offset; position++) {
    const index =
      (centerIndex + position + safeMovies.length) % safeMovies.length;

    visible.push({
      movie: safeMovies[index],
      position,
      index,
    });
  }

  const currentMovie = safeMovies[centerIndex];

  return (
    <section className="relative flex w-full flex-col items-center overflow-visible pb-12 pt-8 select-none">

      {/* Background atmosphere */}
      <div className="pointer-events-none absolute inset-0 overflow-hidden">
        <div className="absolute left-1/2 top-40 h-[500px] w-[500px] -translate-x-1/2 rounded-full bg-red-600/[0.07] blur-[150px]" />
        <div className="absolute bottom-0 left-1/2 h-px w-[80%] -translate-x-1/2 bg-gradient-to-r from-transparent via-red-500/30 to-transparent" />
      </div>

      {/* Wheel decoration */}
      <svg
        className="pointer-events-none absolute left-1/2 top-0 -translate-x-1/2 opacity-60"
        width="1000"
        height="520"
        viewBox="0 0 1000 520"
      >
        <circle
          cx="500"
          cy="920"
          r="650"
          fill="none"
          stroke="rgba(220,38,38,0.13)"
          strokeWidth="1"
        />
        <circle
          cx="500"
          cy="920"
          r="740"
          fill="none"
          stroke="rgba(255,255,255,0.04)"
          strokeWidth="1"
        />
        <circle
          cx="500"
          cy="920"
          r="830"
          fill="none"
          stroke="rgba(255,255,255,0.025)"
          strokeWidth="1"
        />
      </svg>

      {/* Section heading */}
      <div className="relative z-10 mb-2 text-center">
        <p className="text-[10px] font-bold uppercase tracking-[0.5em] text-red-500">
          Your next cinematic experience
        </p>
      </div>

      {/* MOVIE WHEEL - MOVED MUCH LOWER */}
      <div className="relative z-10 mt-64 h-[620px] w-full sm:mt-80 sm:h-[700px]">
        {visible.map(({ movie, position, index }) => {
          const angle = (position / offset) * maxAngle;
          const isCenter = position === 0;
          const distance = Math.abs(position);

          return (
            <button
              key={`${movie.id ?? movie.title ?? 'movie'}-${index}`}
              type="button"
              aria-label={`Select ${movie.title}`}
              onClick={() => {
                if (isCenter) {
                  selectMovie(movie);
                } else {
                  setCenterIndex(index);
                }
              }}
              className="absolute left-1/2 top-[88%] cursor-pointer outline-none focus-visible:ring-2 focus-visible:ring-red-500 focus-visible:ring-offset-4 focus-visible:ring-offset-black"
              style={{
                transform: `
                  translate(-50%, -100%)
                  rotate(${angle}deg)
                  translateY(-${radius}px)
                  rotate(${-angle * 0.35}deg)
                  scale(${isCenter ? 1.1 : 0.88 - distance * 0.035})
                `,
                transformOrigin: 'center center',
                zIndex: isCenter ? 50 : 20 - distance,
                opacity: isCenter ? 1 : distance === 1 ? 0.65 : 0.35,
                filter: isCenter
                  ? 'none'
                  : `grayscale(${distance * 25}%) brightness(${1 - distance * 0.12})`,
                transition:
                  'transform 650ms cubic-bezier(0.22, 1, 0.36, 1), opacity 650ms, filter 650ms',
              }}
            >
              <div
                className={`
                  group relative h-[190px] w-[132px] overflow-hidden rounded-xl
                  bg-[#171717] sm:h-[250px] sm:w-[172px]
                  ${isCenter
                    ? 'shadow-[0_0_35px_rgba(220,38,38,0.22),0_25px_80px_rgba(0,0,0,0.7)]'
                    : 'shadow-[0_15px_40px_rgba(0,0,0,0.5)]'}
                `}
              >
                {movie.posterLink ? (
                  <img
                    src={movie.posterLink}
                    alt={movie.title}
                    draggable="false"
                    className="h-full w-full object-cover transition-transform duration-700 group-hover:scale-105"
                  />
                ) : (
                  <div className="flex h-full w-full flex-col items-center justify-center bg-gradient-to-br from-[#242424] to-[#0a0a0a] p-4 text-center">
                    <span className="mb-3 text-3xl opacity-30">🎬</span>
                    <span className="text-xs font-bold tracking-wider text-gray-400">
                      {movie.title}
                    </span>
                  </div>
                )}

                <div className="absolute inset-0 bg-gradient-to-t from-black/90 via-black/5 to-black/10" />

                {isCenter && (
                  <>
                    <div className="absolute inset-0 rounded-xl border border-red-500/50" />
                    <div className="absolute inset-x-0 bottom-0 h-1 bg-gradient-to-r from-transparent via-red-500 to-transparent" />
                    <div className="absolute inset-0 bg-red-500/[0.025]" />
                  </>
                )}

                <div className="absolute left-3 top-3">
                  <span
                    className={`text-[9px] font-bold tracking-[0.2em] ${
                      isCenter ? 'text-red-400' : 'text-white/40'
                    }`}
                  >
                    {String(index + 1).padStart(2, '0')}
                  </span>
                </div>
              </div>
            </button>
          );
        })}
      </div>

      {/* MOVIE DETAILS - PULLED MUCH HIGHER */}
      <div className="relative z-30 -mt-80 w-full px-4 text-center sm:-mt-[420px]">
        <div className="mx-auto max-w-3xl">
          <h2
            key={currentMovie?.title}
            className="font-display text-3xl font-bold uppercase tracking-[0.08em] text-white sm:text-5xl lg:text-6xl"
          >
            {currentMovie?.title}
          </h2>

          <div className="mt-4 flex items-center justify-center gap-3 text-sm text-gray-400">
            <span>{currentMovie?.releaseYear ?? 'Unknown year'}</span>
            <span className="h-1 w-1 rounded-full bg-red-500" />
            <span className="text-yellow-400">
              ★ {currentMovie?.rating ?? 'N/A'}
            </span>
          </div>

          {/* Controls */}
          <div className="mt-8 flex items-center justify-center gap-4">
            <button
              onClick={prev}
              className="group flex h-12 w-12 items-center justify-center rounded-full border border-white/10 bg-white/[0.03] text-white transition hover:border-red-500/60 hover:bg-red-500/10"
              aria-label="Previous movie"
            >
              <span className="text-xl transition group-hover:-translate-x-0.5">←</span>
            </button>

            <button
              onClick={() => selectMovie(currentMovie)}
              className="rounded-full border border-red-500/40 bg-red-600/10 px-8 py-3 text-xs font-bold uppercase tracking-[0.25em] text-red-400 transition hover:border-red-500 hover:bg-red-600 hover:text-white hover:shadow-[0_0_30px_rgba(220,38,38,0.25)]"
            >
              Select movie
            </button>

            <button
              onClick={next}
              className="group flex h-12 w-12 items-center justify-center rounded-full border border-white/10 bg-white/[0.03] text-white transition hover:border-red-500/60 hover:bg-red-500/10"
              aria-label="Next movie"
            >
              <span className="text-xl transition group-hover:translate-x-0.5">→</span>
            </button>
          </div>

          {/* Instructions */}
          <p className="mt-7 text-[10px] uppercase tracking-[0.3em] text-gray-600">
            <span className="text-gray-400">← →</span>
            <span className="mx-3">Navigate</span>
            <span className="text-gray-400">Enter</span>
            <span className="mx-3">Select</span>
            <span className="text-gray-400">Esc</span>
            <span className="mx-3">Reset</span>
          </p>

          {/* Position indicators */}
          <div className="mt-7 flex items-center justify-center gap-1.5">
            {safeMovies.map((movie, index) => (
              <button
                key={movie.id ?? index}
                onClick={() => setCenterIndex(index)}
                aria-label={`Go to ${movie.title}`}
                className={`h-1 rounded-full transition-all duration-300 ${
                  centerIndex === index
                    ? 'w-7 bg-red-500'
                    : 'w-1.5 bg-white/15 hover:bg-white/40'
                }`}
              />
            ))}
          </div>
        </div>
      </div>
    </section>
  );
}
