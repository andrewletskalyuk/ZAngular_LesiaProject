export class SignUpModel {
    public Email: string = "";
    public Password: string = "";
    public Phone: string = "";
    public Fullname: string = "";
    public Address: string = "";
    public Age: number = 0;

    isValid(): boolean{
        if(
            this.Address!="" ||
            this.Age != 0 || 
            this.Email != "" ||
            this.Fullname != "" ||
            this.Password != "" ||
            this.Phone != ""
        )
        {
            return true;
        }
        else{
            return false;
        }
    }
    isEmail(): boolean {
        const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(String(this.Email).toLowerCase());
    }​​
}