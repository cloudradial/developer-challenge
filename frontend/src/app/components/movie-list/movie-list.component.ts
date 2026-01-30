import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MovieService } from '../../services/movie.service';
import { Movie } from '../../models/movie.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-movie-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './movie-list.component.html',
  styleUrls: ['./movie-list.component.css']
})
export class MovieListComponent implements OnInit {
  movies$!: Observable<Movie[]>;
  loading$!: Observable<boolean>;
  error$!: Observable<string | null>;
  selectedMovie: Movie | null = null;
  sortBy: 'episode' | 'release' | 'title' = 'episode';

  constructor(public movieService: MovieService) {}

  ngOnInit(): void {
    this.loading$ = this.movieService.loading$;
    this.error$ = this.movieService.error$;
    this.movieService.loadMovies();
    this.updateMoviesList();
  }

  selectMovie(movie: Movie): void {
    this.selectedMovie = movie;
  }

  closeCard(): void {
    this.selectedMovie = null;
  }

  onSortChange(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.sortBy = target.value as 'episode' | 'release' | 'title';
    this.updateMoviesList();
  }

  private updateMoviesList(): void {
    this.movies$ = this.movieService.getSortedMovies(this.sortBy);
  }
}
