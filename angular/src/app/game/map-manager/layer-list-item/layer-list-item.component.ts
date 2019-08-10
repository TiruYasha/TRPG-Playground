import { Component, OnInit, Input, ElementRef, ViewChild, Output, EventEmitter, AfterViewChecked, AfterViewInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ValidatorFunctions } from 'src/app/utilities/validator-functions';
import { MatMenuTrigger } from '@angular/material';
import { Layer } from 'src/app/shared/models/map/layer.model';

@Component({
  selector: 'trpg-layer-list-item',
  templateUrl: './layer-list-item.component.html',
  styleUrls: ['./layer-list-item.component.scss']
})
export class LayerListItemComponent implements OnInit {
  @Input() layer: Layer;
  @Input() edit = false;

  @Output() cancelAdd = new EventEmitter<Layer>();
  @Output() completeEdit = new EventEmitter<Layer>();
  @Output() completeAdd = new EventEmitter<Layer>();
  @Output() delete = new EventEmitter<Layer>();

  @ViewChild('nameField') nameField: ElementRef;
  @ViewChild(MatMenuTrigger) trigger: MatMenuTrigger;
  @ViewChild('menuButton') button: ElementRef;

  name = new FormControl('', [Validators.required, ValidatorFunctions.noWhitespaceValidator]);

  constructor() { }

  ngOnInit() {
    if (this.edit) {
      this.intializeEdit();
    }
  }

  renameLayer() {
    if (!this.hasNotOnlyWhiteSpace(this.name.value)) {
      if (this.isNew()) {
        this.layer.name = this.name.value;
        this.completeAdd.emit(this.layer);
        this.edit = false;
      } else {
        this.layer.name = this.name.value;
        this.completeEdit.emit(this.layer);
        this.edit = false;
      }
    } else {
      if (this.isNew()) {
        this.cancelAdd.emit(this.layer);
      } else {
        this.edit = false;
      }
    }
  }

  startRename() {
    this.edit = true;
    this.intializeEdit();
  }

  hasNotOnlyWhiteSpace(value: string) {
    return (value || '').trim().length === 0;
  }

  triggerMenu(event: MouseEvent) {
    event.preventDefault();
    const button = this.button.nativeElement as HTMLDivElement;
    button.style.position = 'absolute';
    button.style.left = `${event.pageX - 64}px`;

    this.trigger.menu.hasBackdrop = true;
    this.trigger.openMenu();
    document.getElementsByClassName('cdk-overlay-backdrop')[0].addEventListener('contextmenu', (offEvent: MouseEvent) => {
      offEvent.preventDefault();
      //Temporary right click fix
      this.trigger.closeMenu();
    });
  }

  private isNew() {
    return this.hasNotOnlyWhiteSpace(this.layer.name);
  }

  private intializeEdit() {
    this.name.setValue(this.layer.name);
    setTimeout(() => {
      this.nameField.nativeElement.focus();
    });
  }
}
