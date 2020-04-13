import { Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { MatDialog, MatTable } from '@angular/material';
import { DialogBoxComponent } from './../dialog-box/dialog-box.component';
import { HomeService, CompanyModel } from './home.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit, OnDestroy {

  displayedColumns: string[] = ['id', 'name', 'exchange', 'ticker', 'isin', 'website', 'action'];
  dataSource: Array<CompanyModel>;

  @ViewChild(MatTable, { static: true }) table: MatTable<any>;

  private companiesSubscription: Subscription;
  private companyByIsinSubscription: Subscription;
  private addSubscription: Subscription;
  private updateSubscription: Subscription;
  private deleteSubscription: Subscription;

  constructor(private readonly dialog: MatDialog, private readonly homeService: HomeService) {
  }

  ngOnInit(): void {
    this.watchCompaniesList();
    if (window.sessionStorage.getItem('userToken')) {
      this.homeService.getCompanies();
    }  else {
      this.homeService.getToken();
    }
  }

  watchCompaniesList(): void {
    this.companiesSubscription = this.homeService.companies$.subscribe(val => {
      this.dataSource = val;
    });
  }

  inputChange(val): void {
    this.dataSource = [];
    if (true) {
      this.companyByIsinSubscription = this.homeService.getCompanyByIsin(val).subscribe(
        company => {
          this.dataSource.push(company);
          this.table.renderRows();
      },
      () => {
        alert('Cannot find company with Isin:' + val + '.');
      });
    } else if (val.length === 0) {
      this.homeService.getCompanies();
    }
  }

  openDialog(action, obj) {

    obj.action = action;
    const dialogRef = this.dialog.open(DialogBoxComponent, {
      width: '250px',
      data: obj
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (result.event === 'Add') {
          this.addRowData(result.data);
        } else if (result.event === 'Update') {
          this.updateRowData(result.data);
        } else if (result.event === 'Delete') {
          this.deleteRowData(result.data);
        }
      }
    });

  }

  addRowData(row_obj) {
    this.addSubscription = this.homeService.addCompanies(row_obj).subscribe(
      val => {
        this.dataSource.push(val);
        this.table.renderRows();
    },
    () => {
      alert('Error adding company.');
    });
  }

  updateRowData(row_obj) {
    this.updateSubscription = this.homeService.updateCompany(row_obj).subscribe(
      () => {
        this.dataSource = this.dataSource.filter(value => {
          if (value.id === row_obj.id) {
            value.name = row_obj.name;
            value.exchange = row_obj.exchange;
            value.ticker = row_obj.ticker;
            value.isin = row_obj.isin;
            value.website = row_obj.website;
          }
          return true;
        });
        this.table.renderRows();
    },
    () => {
      alert('Error updating company.');
    });
  }

  deleteRowData(row_obj) {
    this.deleteSubscription = this.homeService.deleteCompany(row_obj).subscribe(
      () => {
        this.dataSource = this.dataSource.filter(value => {
          return value.id !== row_obj.id;
        });
        this.table.renderRows();
    },
    () => {
      alert('Error deleting company.');
    });
  }

  ngOnDestroy(): void {
    if (this.companiesSubscription) {
      this.companiesSubscription.unsubscribe();
    }
    if (this.companyByIsinSubscription) {
      this.companyByIsinSubscription.unsubscribe();
    }
    if (this.addSubscription) {
      this.addSubscription.unsubscribe();
    }
    if (this.updateSubscription) {
      this.updateSubscription.unsubscribe();
    }
    if (this.deleteSubscription) {
      this.deleteSubscription.unsubscribe();
    }
  }

}
