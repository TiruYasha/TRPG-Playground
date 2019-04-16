import { Component, OnInit, Inject, ViewChild, ElementRef } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { CreateFolderDialogModel } from '../create-folder-dialog/create-folder-dialog.model';
import { fromEvent, Observable, Subscription } from 'rxjs';

@Component({
  selector: 'trpg-parent-dialog',
  templateUrl: './parent-dialog.component.html',
  styleUrls: ['./parent-dialog.component.scss']
})
export class ParentDialogComponent implements OnInit {

  @ViewChild('dialogContainer') dialogContainer: ElementRef;

  startPageX = 0;
  startPageY = 0;
  mouseMove: Observable<Event>;
  mouseMoveSubscription: Subscription;

  mouseUp: Observable<Event>;
  mouseUpSubscription: Subscription;

  constructor(
    public dialogRef: MatDialogRef<ParentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CreateFolderDialogModel
  ) { }

  ngOnInit() {
  }

  exitDialog(): void {
    this.dialogRef.close();
  }

  startResize(event: MouseEvent) {
    this.startPageX = event.pageX;
    this.startPageY = event.pageY;

    this.mouseMove = fromEvent(document, 'mousemove');

    this.mouseMoveSubscription = this.mouseMove
      .subscribe((event: MouseEvent) => {
        this.resize(event);
      });

    this.mouseUp = fromEvent(document, 'mouseup');
    this.mouseUpSubscription = this.mouseUp.subscribe((event: MouseEvent) => this.cancelResize(event));
  }

  resize(event: MouseEvent) {
    const element = this.dialogContainer.nativeElement as HTMLDivElement;
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
