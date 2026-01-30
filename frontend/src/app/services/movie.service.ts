import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { Movie } from '../models/movie.model';

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  private apiUrl = 'http://localhost:5000/api/movies';
  private moviesSubject = new BehaviorSubject<Movie[]>([]);
  private loadingSubject = new BehaviorSubject<boolean>(false);
  private errorSubject = new BehaviorSubject<string | null>(null);

  public movies$ = this.moviesSubject.asObservable();
  public loading$ = this.loadingSubject.asObservable();
  public error$ = this.errorSubject.asObservable();

  constructor(private http: HttpClient) {}

  loadMovies(): void {
    this.loadingSubject.next(true);
    this.errorSubject.next(null);

    this.http.get<Movie[]>(this.apiUrl).pipe(
      tap(movies => {
        this.moviesSubject.next(movies);
        this.loadingSubject.next(false);
      }),
      catchError(error => {
        const errorMessage = 'Failed to load movies. Please try again later.';
        this.errorSubject.next(errorMessage);
        this.loadingSubject.next(false);
        return throwError(() => error);
      })
    ).subscribe();
  }

  getMovieById(episodeId: number): Observable<Movie | undefined> {
    return this.movies$.pipe(
      map(movies => movies.find(m => m.episodeId === episodeId))
    );
  }

  getSortedMovies(sortBy: 'episode' | 'release' | 'title'): Observable<Movie[]> {
    return this.movies$.pipe(
      map(movies => {
        const sorted = [...movies];
        switch (sortBy) {
          case 'episode':
            return sorted.sort((a, b) => a.episodeId - b.episodeId);
          case 'release':
            return sorted.sort((a, b) =>
              new Date(a.releaseDate).getTime() - new Date(b.releaseDate).getTime()
            );
          case 'title':
            return sorted.sort((a, b) => a.title.localeCompare(b.title));
          default:
            return sorted;
        }
      })
    );
  }
}
