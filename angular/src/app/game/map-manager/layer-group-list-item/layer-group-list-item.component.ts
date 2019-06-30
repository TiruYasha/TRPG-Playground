import { Component, OnInit, Input, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { Layer } from 'src/app/models/map/layer.model';
import { MatMenuTrigger } from '@angular/material';
import { LayerGroup } from 'src/app/models/map/layer-group.model';
import { FormControl, Validators } from '@angular/forms';
import { ValidatorFunctions } from 'src/app/utilities/validator-functions';

@Component({
  selector: 'trpg-layer-group-list-item',
  templateUrl: './layer-group-list-item.component.html',
  styleUrls: ['./layer-group-list-item.component.scss']
})
export class LayerGroupListItemComponent implements OnInit {

  @Input() layer: LayerGroup;
  @Input() edit = false;

  @Output() cancelAdd = new EventEmitter<Layer>();
  @Output() completeEdit = new EventEmitter<Layer>();
  @Output() completeAdd = new EventEmitter<Layer>();
  @Output() delete = new EventEmitter<Layer>();

  @ViewChild('groupNameField') nameField: ElementRef;
  @ViewChild(MatMenuTrigger) trigger: MatMenuTrigger;
  @ViewChild('menuButton') button: ElementRef;

  editLayer: Layer;
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

  onAddNewSubLayer() {
    const layer = new Layer();

    this.layer.layers.push(layer);
    this.editLayer = layer;
  }

  completeAddSubLayer(layer: Layer) {
    layer.LayerGroupId = this.layer.id;
    this.completeAdd.emit(layer);
  }

  cancelSubLayerAdd(layer: Layer) {
    this.layer.layers = this.layer.layers.filter(l => l !== layer);
  }

  deleteSubLayer(layer: Layer) {
    this.delete.emit(layer);
    this.layer.layers = this.layer.layers.filter(l => l !== layer);
  }

  private isNew() {
    return this.hasNotOnlyWhiteSpace(this.layer.name);
  }

  private intializeEdit() {
    this.name.setValue(this.layer.name);
    this.nameField.nativeElement.focus();
  }
}
