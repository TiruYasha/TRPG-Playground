import { Component, Input, Output, EventEmitter } from '@angular/core';
import { speedDialFabAnimations } from './speed-dial-fab.animations';

@Component({
  selector: 'trpg-speed-dial-fab',
  templateUrl: './speed-dial-fab.component.html',
  styleUrls: ['./speed-dial-fab.component.scss'],
  animations: speedDialFabAnimations
})
export class SpeedDialFabComponent {
  @Input() mainIcon: string;
  @Input() subIcons: string[];

  @Output() clickSubIcon = new EventEmitter<string>();

  buttons = [];
  isToggled = 'inactive';

  constructor() { }

  showItems() {
    this.isToggled = 'active';
    this.buttons = this.subIcons;
  }

  hideItems() {
    this.isToggled = 'inactive';
    this.buttons = [];
  }

  onToggleFab() {
    this.buttons.length ? this.hideItems() : this.showItems();
  }

  subIconClicked(icon: string) {
    this.clickSubIcon.emit(icon);
  }
}
