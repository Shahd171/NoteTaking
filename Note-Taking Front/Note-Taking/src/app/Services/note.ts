import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_CONFIG } from '../app.config';
export interface noteInterface {
  id: number;
  title: string;
  content: string;
  createdAt: string;
  updatedAt: string | null;
  username: string;
  color?: string; // Optional in case you add custom colors later
}
@Injectable({
  providedIn: 'root'
})

export class Note {
    constructor(private http: HttpClient) {}
 getAllNotes(): Observable<{ success: boolean, message: string, data: noteInterface[] }> {
    return this.http.get<{ success: boolean, message: string, data: noteInterface[] }>(`${API_CONFIG.apiUrl}/api/Notes`);
  }
  createNote(dto: { title: string; content: string ;color?: string}): Observable<any> {
    console.log(dto.color);
  return this.http.post(`${API_CONFIG.apiUrl}/api/Notes`, dto);
}
updateNote(id: number, dto: { title: string; content: string; color?: string }): Observable<any> {
  return this.http.put(`${API_CONFIG.apiUrl}/api/Notes/${id}`, dto);
}
deleteNote(id: number): Observable<any> {
  return this.http.delete(`${API_CONFIG.apiUrl}/api/Notes/${id}`);
}

}
