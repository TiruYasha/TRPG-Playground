import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ActiveGameService {
  gameId: string;
  constructor() { }
}
