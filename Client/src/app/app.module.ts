import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppComponent} from './app.component';
import {RouterOutlet} from "@angular/router";
import {HomeComponent} from './components/home/home.component';
import {AppRoutingModule} from "./app-routing.module";
import {UploadComponent} from './components/upload/upload.component';
import {ArchiveComponent} from './components/archive/archive.component';
import {CdkTableModule} from "@angular/cdk/table";
import {ArchiveTableItemComponent} from './components/archive-table-item/archive-table-item.component';
import {HttpClientModule} from "@angular/common/http";
import {ReactiveFormsModule} from "@angular/forms";
import { FillPipe } from './pipes/fill.pipe';
import { ArchiveHourlyComponent } from './components/archive-hourly/archive-hourly.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    UploadComponent,
    ArchiveComponent,
    ArchiveTableItemComponent,
    FillPipe,
    ArchiveHourlyComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    RouterOutlet,
    CdkTableModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
