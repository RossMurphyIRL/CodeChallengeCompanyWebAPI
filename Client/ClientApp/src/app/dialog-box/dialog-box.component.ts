import { Component, Inject, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog, MatTable  } from '@angular/material';


export interface UsersData {
  name: string;
  id: number;
}

@Component({
  selector: 'app-dialog-box',
  templateUrl: './dialog-box.component.html',
  styleUrls: ['./dialog-box.component.css']
})

export class DialogBoxComponent {

  action: string;
  local_data: any;

  constructor(
    public dialogRef: MatDialogRef<DialogBoxComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any) {
    this.local_data = {...data};
    this.action = this.local_data.action;
  }

  doAction() {
    this.dialogRef.close({event: this.action, data: this.local_data});
  }

  closeDialog() {
    this.dialogRef.close({event: 'Cancel'});
  }

  get formInvalid(): boolean {
    return !this.local_data.name || !this.local_data.exchange || !this.local_data.ticker ||
           !this.local_data.isin || this.local_data.isin.length !== 12 ||
           (this.local_data.isin.length === 12 && !/^[a-zA-Z]+$/.test(this.local_data.isin.substring(0, 2)));
  }

}
