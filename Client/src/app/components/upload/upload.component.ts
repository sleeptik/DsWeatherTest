import {Component, ElementRef, ViewChild} from '@angular/core';
import {BehaviorSubject, delay, tap} from "rxjs";
import {ArchiveService} from "../../services/archive.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html'
})
export class UploadComponent {
  @ViewChild("fileInput", {static: true}) readonly fileInputElement!: ElementRef;
  readonly files: BehaviorSubject<File[]> = new BehaviorSubject<File[]>([]);

  constructor(
    private readonly archiveService: ArchiveService,
    private readonly router: Router
  ) {
  }

  private get input() {
    return this.fileInputElement.nativeElement as HTMLInputElement;
  }

  selectFiles() {
    this.input.click();
  }

  filesSelected() {
    if (this.input.files)
      this.setFiles(this.input.files);
  }

  dragOver($event: DragEvent) {
    $event.preventDefault();
  }

  drop($event: DragEvent) {
    $event.preventDefault();
    if ($event.dataTransfer?.files)
      this.setFiles($event.dataTransfer.files);
  }

  uploadFiles() {
    this.archiveService.uploadArchives(this.files.value)
      .pipe(tap(_ => this.files.next([])))
      .pipe(delay(250))
      .subscribe(_ => this.router.navigateByUrl("/  "));
  }

  private setFiles(list: FileList) {
    if (!list || list.length === 0)
      return;

    const fileList: File[] = [];
    for (let i = 0; i < list.length; i++) {
      const file = list.item(i);

      if (!file || file.type !== "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        continue;

      fileList.push(file);
    }

    this.files.next(fileList);
  }
}
