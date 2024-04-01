import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomeComponent} from "./components/home/home.component";
import {UploadComponent} from "./components/upload/upload.component";
import {ArchiveComponent} from "./components/archive/archive.component";

const routes: Routes = [
  {path: "archive", component: ArchiveComponent},
  {path: "upload", component: UploadComponent},
  {path: "", component: HomeComponent, pathMatch: "full"}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
