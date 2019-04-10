import { Component, OnInit, Input } from '@angular/core';
import { NestedTreeControl } from '@angular/cdk/tree';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { JournalItem } from 'src/app/models/journal/receives/journal-item.model';
import { JournalService } from './journal.service';
import { CreateFolderDialogComponent } from './create-folder-dialog/create-folder-dialog.component';
import { MatDialog, getMatFormFieldDuplicatedHintError } from '@angular/material';
import { AddJournalFolderRequestModel } from 'src/app/models/journal/requests/AddJournalFolderRequest.model';
import { Guid } from 'src/app/utilities/guid.util';

@Component({
  selector: 'trpg-journal',
  templateUrl: './journal.component.html',
  styleUrls: ['./journal.component.scss']
})
export class JournalComponent implements OnInit {
  @Input() isOwner: boolean;
  @Input() gameId: string;

  journalItems: JournalItem[] = [];

  subIcons = ['create_new_folder', 'person_add', 'note_add'];

  constructor(private journalService: JournalService, public dialog: MatDialog) {
  }

  ngOnInit() {
    this.journalService.setup(this.gameId);
  }

  subIconClicked(icon: string) {
    if (icon === 'create_new_folder') {
      this.openCreateNewFolderDialog();
    }
  }

  private openCreateNewFolderDialog() {
    const dialogRef = this.dialog.open(CreateFolderDialogComponent, {
      width: '250px',
      data: { name: '' }
    });

    dialogRef.afterClosed().subscribe((folderName) => this.createNewFolder(folderName));
  }

  private createNewFolder(folderName) {
    if (folderName === void 0) {
      return;
    }

    const folderRequest: AddJournalFolderRequestModel =  {
      name: folderName,
      gameId: this.gameId,
      parentFolder: Guid.getEmptyGuid()
    };

    this.journalService.addFolderToGame(folderRequest);
  }
}
