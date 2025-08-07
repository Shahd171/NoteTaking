import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Login } from './Components/login/login';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet,HttpClientModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Note-Taking');
  
}
