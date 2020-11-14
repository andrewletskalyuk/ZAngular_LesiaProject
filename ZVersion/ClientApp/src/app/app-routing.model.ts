import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule, CanActivate } from '@angular/router';
import { HomeComponent} from './home/home.component';
import { SignInComponent } from './signIn/signIn.component';
import { SignUpComponent } from './signUp/signUp.component';
// import { AdminAreaComponent } from './Admin-Area/Admin-Area.component';
// import { ClientAreaComponent } from './ClientArea/ClientArea.component';
// import { AdminGuard } from './guards/admin.guard';
 import { notLoginGuard } from './guards/notLogin.guard';
 import { loggedInGuard } from './guards/loggedIn.guard';
// import { ListProductComponent } from './Admin-Area/list-product/list-product.component';
// import { AddProductComponent } from './Admin-Area/add-product/add-product.component';

const routes: Routes = [
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'sign-in', canActivate: [notLoginGuard], pathMatch: 'full', component: SignInComponent},
    { path: 'sign-up', canActivate: [notLoginGuard], pathMatch: 'full', component: SignUpComponent},
    // { 
    //     path: 'admin-panel',
    //     canActivate: [AdminGuard],
    //     component: AdminAreaComponent,
    //     children:
    //     [
    //         {path: '', component: ListProductComponent, pathMatch: 'full'},
    //         {path: 'add-product', component: AddProductComponent , pathMatch: 'full'}
    //     ]
    // },
    // { path: 'client-panel', canActivate: [loggedInGuard], pathMatch: 'full', component: ClientAreaComponent}
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})

export class AppRoutingModule { }