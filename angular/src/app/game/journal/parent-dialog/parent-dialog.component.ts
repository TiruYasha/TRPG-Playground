import { Component, OnInit, Inject, ViewChild, ElementRef } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { fromEvent, Observable, Subscription } from 'rxjs';
import { ParentDialogModel } from './parent-dialog.model';
import { JournalService } from '../../../shared/services/journal.service';
import { DestroySubscription } from 'src/app/shared/components/destroy-subscription.extendable';
import { DialogState } from '../../../shared/models/dialog-state.enum';
import { takeUntil } from 'rxjs/operators';
import { JournalItemType } from 'src/app/shared/models/journal/journalitems/journal-item-type.enum';
import { JournalItem } from 'src/app/shared/models/journal/journalitems/journal-item.model';
import { AddJournalItemRequestModel } from 'src/app/shared/models/journal/requests/add-journal-item-request.model';
import { JournalCharacterSheet } from 'src/app/shared/models/journal/journalitems/journal-character-sheet.model';

@Component({
  selector: 'trpg-journal-parent-dialog',
  templateUrl: './parent-dialog.component.html',
  styleUrls: ['./parent-dialog.component.scss']
})
export class ParentDialogComponent extends DestroySubscription implements OnInit {
  @ViewChild('dialogContainer') dialogContainer: ElementRef;

  startPageX = 0;
  startPageY = 0;
  mouseMove: Observable<Event>;
  mouseMoveSubscription: Subscription;

  mouseUp: Observable<Event>;
  mouseUpSubscription: Subscription;

  handoutType = JournalItemType;
  states = DialogState;

  journalItem: JournalItem;
  isValid = false;

  constructor(
    public dialogRef: MatDialogRef<ParentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ParentDialogModel,
    private journalService: JournalService
  ) { super(); }

  ngOnInit() {
    if (this.data.state === DialogState.View || this.data.state === DialogState.Edit) {
      this.loadJournalItemDetails();
    }

    if (this.data.state === DialogState.New) {
      this.journalItem = new JournalItem(this.data.journalItemType);
    }

    this.journalService.journalItemUpdated.pipe(takeUntil(this.destroy))
      .subscribe(data => {
        if (data.id === this.journalItem.id) {
          this.loadJournalItemDetails();
        }
      });
  }

  saveJournalItem() {
    const journalItem = this.journalItem;
    if (this.data.state === DialogState.New) {
      const request: AddJournalItemRequestModel = {
        parentFolderId: this.data.parentFolderId,
        journalItem: journalItem
      };
      this.journalService.addJournalItemToGame(request).pipe(takeUntil(this.destroy))
        .subscribe((i) => {
          if (this.journalItem.type !== JournalItemType.Folder) {
            this.journalItem.id = i.id;
            this.data.journalItemId = i.id;
            this.data.state = DialogState.Edit;
          } else {
            this.exitDialog();
          }
        });
    } else {
      this.journalService.saveJournalItem(journalItem).pipe(takeUntil(this.destroy))
        .subscribe(() => {
          if (this.data.journalItemType === JournalItemType.Folder) {
            this.exitDialog();
          } else {
            this.data.state = DialogState.View;
          }
        });
    }
  }

  uploadImage() {
    if (this.journalItem.image) {
      this.journalService.uploadImage(this.journalItem.id, this.journalItem.image)
        .pipe(takeUntil(this.destroy))
        .subscribe(() => {
          //TODO message that uploading image is successful.
        });
    }
  }

  uploadToken() {
    const characterSheet = this.journalItem as JournalCharacterSheet;
    if (characterSheet.token) {
      const characterSheet = this.journalItem as JournalCharacterSheet;
      this.journalService.uploadToken(characterSheet.id, characterSheet.token)
        .pipe(takeUntil(this.destroy))
        .subscribe(() => {
          //TODO message that uploading image is successful.
        });
    }
  }

  exitDialog(): void {
    this.dialogRef.close();
  }

  startResize(event: MouseEvent) {
    this.startPageX = event.pageX;
    this.startPageY = event.pageY;
    this.mouseMove = fromEvent(document, 'mousemove');

    this.mouseMoveSubscription = this.mouseMove
      .pipe(takeUntil(this.destroy))
      .subscribe((next: MouseEvent) => {
        this.resize(next);
      });

    this.mouseUp = fromEvent(document, 'mouseup');
    this.mouseUpSubscription = this.mouseUp
      .pipe(takeUntil(this.destroy))
      .subscribe((next: MouseEvent) => this.cancelResize(next));
  }

  resize(event: MouseEvent) {
    const element = this.dialogContainer.nativeElement as HTMLDivElement;

    event.preventDefault();
    const minWidth = this.startPageX - event.pageX;
    const minHeight = this.startPageY - event.pageY;

    this.startPageX = event.pageX;
    this.startPageY = event.pageY;

    element.style.height = (element.clientHeight - minHeight) + 'px';
    element.style.width = (element.clientWidth - minWidth - 48) + 'px';
  }

  cancelResize(event: MouseEvent) {
    this.mouseMoveSubscription.unsubscribe();
    this.mouseUpSubscription.unsubscribe();
  }

  private loadJournalItemDetails() {
    this.journalService.getJournalItemById(this.data.journalItemId)
      .pipe(takeUntil(this.destroy))
      .subscribe(data => this.journalItem = data);
  }
}
