import { Routes } from '@angular/router';
import { Login } from './Components/login/login';
import { Register } from './Components/register/register';
import { Home } from './Components/home/home';

export const routes: Routes = [
    {path:'',redirectTo:"register",pathMatch:'full'},
    {path:"login",component:Login},
    {path:"register",component:Register},
    {path:"Home",component:Home}
];
