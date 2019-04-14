import { Injectable } from '@angular/core';
import { Guid } from 'src/app/utilities/guid.util';

@Injectable({
    providedIn: 'root'
})
export class ActiveGameService {
    public activeGameId = Guid.getEmptyGuid();
}
