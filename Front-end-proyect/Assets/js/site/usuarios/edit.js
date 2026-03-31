const formEditUser = document.getElementById("form-edi-User");
const InputName = document.getElementById("Name");
const InputLastName = document.getElementById("LastName");
const InputEmail = document.getElementById("email");
const InputUserName = document.getElementById("UserName");
const InputPassword = document.getElementById("Password");
const InputTelefono = document.getElementById("tel");
const InputImagen = document.getElementById("imagePerfil");




const params = new URLSearchParams(window.location.search);
const id = params.get("id");


async function GetUser(){
try{
   
    const token = localStorage.getItem("token");
    const result = await axios.get(`https://localhost:7039/Api/v1/ManagerAccount/GetById/${id}`,{
        headers: {
            Authorization: `Bearer ${token}`
        }

    });
    

    fillForm(result.data);

}catch(err){
 
     console.log("Ocurrio un error al intentar tener el usuario");
     
}
}




function fillForm(data){
inputName.value = data.name;
InputLastName.value = data.lastName;
inputTelefono.value = data.telefono;
InputEmail.value = data.email;
InputPassword.value = data.password;
inputImage.value = data.image;
InputUserName.value = data.userName;
}



formEditUser.addEventListener("submit",function(e){
 e.preventDefault();
  editUser();

});



async function editUser(name,lastName,email,userName,password,telefono,Profileimage){

    try{
          
        const data = new FormData();
        data.append("Name",name);
        data.append("LasName",lastName);
        data.append("Email",email);
        data.append("UserName",userName);
        data.append("Password",password);
        data.append("Telefono",telefono);
        data.append("ProfileImage",Profileimage); 


        const token = localStorage.getItem("token");
        const result = await axios.put(`https://localhost:7039/Api/v1/ManagerAccount/EditUser/${id}`,data,{
            
             headers: {
              
                 Authorization: `Bearer ${token}`

             }
        });

           
      
    }catch(err){
       
        console.log("Ocurrio un error al intentar actualizar el usuario");

    }

}

