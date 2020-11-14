import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';


//import { AdminAreaComponent } from './Admin-Area/Admin-Area.component';
//import { ClientAreaComponent } from './ClientArea/ClientArea.component';
import { AppRoutingModule } from './app-routing.model';
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { NotifierModule, NotifierOptions} from 'angular-notifier';
import { DemoNgZorroAntdModule } from './ng-zorro.module';

//icons:
import { NZ_ICONS } from 'ng-zorro-antd/icon';
import { IconDefinition } from '@ant-design/icons-angular';
import * as AllIcons from '@ant-design/icons-angular/icons';
//import { ListProductComponent } from './Admin-Area/list-product/list-product.component';
//import { AddProductComponent } from './Admin-Area/add-product/add-product.component';
import { TokenInterceptor } from './interceptor';
import { SignInComponent } from './signIn/signIn.component';
import { SignUpComponent } from './signUp/signUp.component';

const antDesignIcons = AllIcons as {
  [key: string]: IconDefinition;
};
const icons: IconDefinition[] = Object.keys(antDesignIcons).map(key => antDesignIcons[key])

const conf: NotifierOptions = {
  position: {
    horizontal: {
      position: 'right'
    },
    vertical: {
      position: 'top'
    }
  }
};

@NgModule({
  declarations: [		
    AppComponent,
    NavMenuComponent,
    HomeComponent,
      SignInComponent,
      SignUpComponent,
      //AdminAreaComponent,
      //ClientAreaComponent,
      //ListProductComponent,
,      //AddProductComponent
      SignUpComponent
   ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    NgxSpinnerModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    NotifierModule.withConfig(conf),
    DemoNgZorroAntdModule,
  ],
  providers: [
    NgxSpinnerService,
    {provide: NZ_ICONS, useValue: icons},
    {provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
