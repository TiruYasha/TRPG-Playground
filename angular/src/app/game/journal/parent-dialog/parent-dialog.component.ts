import { Component, OnInit, Inject, ViewChild, ElementRef } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { fromEvent, Observable, Subscription } from 'rxjs';
import { ParentDialogModel } from './parent-dialog.model';
import { JournalItem } from 'src/app/models/journal/journalitems/journal-item.model';
import { JournalItemType } from 'src/app/models/journal/journalitems/journal-item-type.enum';
import { JournalService } from '../journal.service';
import { JournalHandout } from 'src/app/models/journal/journalitems/journal-handout.model';
import { AddJournalItemRequestModel } from 'src/app/models/journal/requests/add-journal-folder-request.model';
import { DestroySubscription } from 'src/app/shared/components/destroy-subscription.extendable';
import { DialogState } from './dialog-state.enum';
import { takeUntil } from 'rxjs/operators';

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

  journalItem: Observable<JournalItem>;

  constructor(
    public dialogRef: MatDialogRef<ParentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ParentDialogModel,
    private journalService: JournalService
  ) { super(); }

  ngOnInit() {
    if (this.data.state === DialogState.View) {
      this.journalItem = this.journalService.getJournalItemById(this.data.journalItemId);
    }
  }

  saveJournalItem(journalItem: JournalItem) {
    switch (journalItem.type) {
      case JournalItemType.Folder:
        this.saveItem(journalItem);
        break;
      case JournalItemType.Handout:
        this.saveHandout(journalItem as JournalHandout);
        break;
    }
  }

  saveItem(journalItem: JournalItem) {
    const request: AddJournalItemRequestModel = {
      parentFolderId: this.data.parentFolderId,
      journalItem: journalItem
    };
    this.journalService.addJournalItemToGame(request).subscribe(() => this.exitDialog());
  }

  saveHandout(journalItem: JournalHandout) {
    const request: AddJournalItemRequestModel = {
      parentFolderId: this.data.parentFolderId,
      journalItem: journalItem
    };

    this.journalService.addJournalItemToGame(request)
      .subscribe(i => {
        this.journalService.uploadImage(i.id, journalItem.image)
          .subscribe(e => this.exitDialog());
      });
  }

  exitDialog(): void {
    this.dialogRef.close();
  }

  startResize(event: MouseEvent) {
    this.startPageX = event.pageX;
    this.startPageY = event.pageY;

    this.mouseMove = fromEvent(document, 'mousemove');

    this.mouseMoveSubscription = this.mouseMove
      .subscribe((next: MouseEvent) => {
        this.resize(next);
      });

    this.mouseUp = fromEvent(document, 'mouseup');
    this.mouseUpSubscription = this.mouseUp.subscribe((next: MouseEvent) => this.cancelResize(next));
  }

  resize(event: MouseEvent) {
    const element = this.dialogContainer.nativeElement as HTMLDivElement;
    console.log(element);
    event.preventDefault();
    const minWidth = this.startPageX - event.pageX;
    const minHeigth = this.startPageY - event.pageY;

    this.startPageX = event.pageX;
    this.startPageY = event.pageY;

    element.style.height = (element.clientHeight - minHeigth - 48) + 'px';
    element.style.width = (element.clientWidth - minWidth - 48) + 'px';
  }

  cancelResize(event: MouseEvent) {
    this.mouseMoveSubscription.unsubscribe();
    this.mouseUpSubscription.unsubscribe();
  }
}
