import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { Note, noteInterface } from '../../Services/note';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../Services/auth';
import { Router } from '@angular/router';
declare var bootstrap: any; // Required for Bootstrap modal handling

@Component({
  selector: 'app-home',
  imports: [CommonModule,FormsModule],
  templateUrl: './home.html',
  styleUrl: './home.css'
})

export class Home implements OnInit {
   selectedColor: string = '#f8f9fa'; // default note color
  colorOptions: string[] = ['#f9f871', '#a5d8ff', '#ff6b6b','#06d6a0', '#9d4edd', '#f72585', '#48cae4', '#fca311'];
  notes: noteInterface[] = [];
  newNote = {
  title: '',
  content: '',
    color: this.selectedColor
};
editMode: boolean = false;
editingNoteId: number | null = null;

constructor(private noteService: Note,private authService:Auth, @Inject(PLATFORM_ID) private platformId: Object,private router: Router
) {}

  ngOnInit(): void {
 if (isPlatformBrowser(this.platformId)) {
      this.loadNotes();
    }  }

  loadNotes(): void {
    console.log("tokennn");
    console.log(this.authService.getToken());
    this.noteService.getAllNotes().subscribe({
      next: (res) => {
        this.notes = res.data.map((note, index) => ({
          ...note,
        color: note.color || this.colorOptions[index % this.colorOptions.length] // ✅ only assign if color is missing
        }));
      },
      error: (err) => {
        console.error('Failed to load notes', err);
      }
    });
  }



submitNewNote() {
  const noteData = {
    title: this.newNote.title,
    content: this.newNote.content,
    color: this.selectedColor
  };

  const modalElement = document.getElementById('addNoteModal');
  const modalInstance = modalElement ? bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement) : null;

  if (this.editMode && this.editingNoteId !== null) {
    // Update existing note
    this.noteService.updateNote(this.editingNoteId, noteData).subscribe({
      next: () => {
        this.loadNotes();
        this.resetForm();
        modalInstance?.hide();
      },
      error: (err) => {
        console.error('Failed to update note', err);
      }
    });
  } else {
    // Create new note
    this.noteService.createNote(noteData).subscribe({
      next: () => {
        this.loadNotes();
        this.resetForm();
        modalInstance?.hide();
      },
      error: (err) => {
        console.error('Failed to create note', err);
      }
    });
  }
}
resetForm() {
  this.editMode = false;
  this.editingNoteId = null;
  this.newNote = { title: '', content: '', color: this.colorOptions[0] };
  this.selectedColor = this.colorOptions[0];
}

editNote(note: noteInterface) {
  this.editMode = true;
  this.editingNoteId = note.id;

  // Populate form
  this.newNote = {
    title: note.title,
    content: note.content,
    color: note.color || this.selectedColor
  };
  this.selectedColor = this.newNote.color;

  const modalElement = document.getElementById('addNoteModal');
  if (modalElement) {
    const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
    modalInstance.show();
  }
}
logout() {
  this.authService.logout(); // this handles clearing token and redirect
}
deleteNoteId: number | null = null;

openDeleteModal(noteId: number): void {
  this.deleteNoteId = noteId;

  const modalElement = document.getElementById('deleteConfirmModal');
  if (modalElement) {
    const modalInstance = new bootstrap.Modal(modalElement);
    modalInstance.show();
  }
}

confirmDelete(): void {
  if (this.deleteNoteId !== null) {
    this.noteService.deleteNote(this.deleteNoteId).subscribe({
      next: () => {
        this.loadNotes();

        // Close modal
        const modalElement = document.getElementById('deleteConfirmModal');
        if (modalElement) {
          const modalInstance = bootstrap.Modal.getInstance(modalElement);
          modalInstance?.hide();
        }

        this.deleteNoteId = null; // Reset
      },
      error: (err) => {
        console.error('Failed to delete note:', err);
        alert("Failed to delete note.");
      }
    });
  }
}


}
