import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { takeUntil } from 'rxjs/operators';
import { DestroySubscription } from 'src/app/shared/components/destroy-subscription.extendable';
import { MapService } from '../../services/map.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ValidatorFunctions } from 'src/app/utilities/validator-functions';
import { PlayMap } from 'src/app/models/map/map.model';
import { AddMap } from 'src/app/models/map/requests/add-map.model';
import { CreateMapDialogModel } from './create-map-dialog.model';
import { DialogState } from 'src/app/models/dialog-state.enum';

@Component({
  selector: 'trpg-create-map-dialog',
  templateUrl: './create-map-dialog.component.html',
  styleUrls: ['./create-map-dialog.component.scss']
})
export class CreateMapDialogComponent extends DestroySubscription implements OnInit {

  form = new FormGroup({
    name: new FormControl('', [Validators.required, ValidatorFunctions.noWhitespaceValidator]),
    gridSizeInPixels: new FormControl(50, [Validators.max(4000), Validators.min(0)]),
    heightInPixels: new FormControl(1080, [Validators.max(4000), Validators.min(0)]),
    widthInPixels: new FormControl(1920, [Validators.max(4000), Validators.min(0)])
  });

  get name() { return this.form.get('name'); }
  get gridSizeInPixels() { return this.form.get('gridSizeInPixels'); }
  get heightInPixels() { return this.form.get('heightInPixels'); }
  get widthInPixels() { return this.form.get('widthInPixels'); }

  constructor(
    public dialogRef: MatDialogRef<CreateMapDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CreateMapDialogModel,
    private mapService: MapService
  ) { super(); }

  ngOnInit() {
    if (this.data.dialogState === DialogState.Edit) {
      this.name.setValue(this.data.map.name);
      this.gridSizeInPixels.setValue(this.data.map.gridSizeInPixels);
      this.heightInPixels.setValue(this.data.map.heightInPixels);
      this.widthInPixels.setValue(this.data.map.widthInPixels);
    }
  }

  saveMap() {
    if (!this.form.valid) {
      return;
    }

    if (this.data.dialogState === DialogState.New) {
      this.addMap();
    } else if (this.data.dialogState === DialogState.Edit) {
      this.updateMap();
    }

  }

  updateMap() {
    const map: PlayMap = {
      id: this.data.map.id,
      name: this.name.value,
      gridSizeInPixels: this.gridSizeInPixels.value,
      heightInPixels: this.heightInPixels.value,
      widthInPixels: this.widthInPixels.value
    };

    this.mapService.updateMap(map)
      .pipe(takeUntil(this.destroy))
      .subscribe(() => {
        this.dialogRef.close(map);
      });
  }

  addMap() {
    const addMap: AddMap = {
      name: this.name.value,
      gridSizeInPixels: this.gridSizeInPixels.value,
      heightInPixels: this.heightInPixels.value,
      widthInPixels: this.widthInPixels.value
    };

    this.mapService.addMap(addMap)
      .pipe(takeUntil(this.destroy))
      .subscribe((map) => {
        this.dialogRef.close(map);
      });
  }

  exitDialog() {
    this.dialogRef.close(null);
  }
}
