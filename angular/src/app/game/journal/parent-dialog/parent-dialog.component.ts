import { Component, OnInit, Inject, ViewChild, ElementRef } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { fromEvent, Observable, Subscription } from 'rxjs';
import { ParentDialogModel } from './parent-dialog.model';
import { JournalItem } from 'src/app/models/journal/journalitems/journal-item.model';
import { JournalItemType } from 'src/app/models/journal/journalitems/journal-item-type.enum';
import { JournalService } from '../journal.service';
import { JournalHandout } from 'src/app/models/journal/journalitems/journal-handout.model';
import { AddJournalItemRequestModel } from 'src/app/models/journal/requests/add-journal-item-request.model';
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
          if (this.journalItem.image) {
            this.journalService.uploadImage(i.id, journalItem.image)
              .pipe(takeUntil(this.destroy))
              .subscribe(e => this.exitDialog());
          } else {
            this.exitDialog();
          }
        });
    } else {
      journalItem.id = this.data.journalItemId;
      this.journalService.saveJournalItem(journalItem).pipe(takeUntil(this.destroy))
        .subscribe(() => {
          if (this.data.journalItemType === JournalItemType.Folder) {
            this.exitDialog();
          } else {
            if (this.journalItem.image) {
              this.journalService.uploadImage(this.journalItem.id, journalItem.image)
                .pipe(takeUntil(this.destroy))
                .subscribe(() => {
                  this.journalItem = journalItem;
                  this.data.state = DialogState.View;
                });
            }
          }
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
    const minHeigth = this.startPageY - event.pageY;

    this.startPageX = event.pageX;
    this.startPageY = event.pageY;

    element.style.height = (element.clientHeight - minHeigth) + 'px';
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
