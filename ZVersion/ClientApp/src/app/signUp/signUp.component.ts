import { AuthService } from './../Services/Auth.service';
import { Component, OnInit } from '@angular/core';
import { SignUpModel } from '../Models/sign-up.model';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotifierService } from 'angular-notifier';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signUp',
  templateUrl: './signUp.component.html',
  styleUrls: ['./signUp.component.css']
})
export class SignUpComponent implements OnInit {
  constructor(
    private spinner: NgxSpinnerService,
    private notifier: NotifierService,
    private authService: AuthService,
    private router: Router
  ) { }
  model = new SignUpModel();
  confirmPassword: string;

  register() {
    if (!this.model.isValid()) {
      this.spinner.hide();
      this.notifier.notify('error', 'Please enter all fields');
    }
    else if(!this.model.isEmail()){
      this.spinner.hide();
      this.notifier.notify('error', 'Your email incorrect');
    }
    else if(this.model.Password != this.confirmPassword)
    {
      this.spinner.hide();
      this.notifier.notify('error', 'Password don\'t match');
    }
    else{
      this.authService.SingUp(this.model).subscribe(
        data => {
          if (data.status === 200) {
            this.notifier.notify('success','You success registered!');
            this.router.navigate(['/sing-in']);
          }
          else{
            for (var i = 0; i < data.errors.length; i++) {
              this.notifier.notify('error', data.errors[i]);
            }
          }

          setTimeout(()=>{
            this.spinner.hide();
          }, 1000)
        }
      )
    }
  }

  ngOnInit() {
  }

}
