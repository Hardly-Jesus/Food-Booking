const formAddPago = document.getElementById("form-add-pago");




const params = new URLSearchParams(window.location.search);
const id = Number(params.get("id"));




formAddPago.addEventListener("submit",function(e){
   e.preventDefault();
        
      const InputMonto = document.getElementById("monto");
      const idPedido = id;
      AddPago(InputMonto.value,idPedido);

});




async function AddPago(monto,idPedido)
{
    try
    {
       
               
       const token = localStorage.getItem("token"); 
       const result = await axios.post("https://localhost:7039/Api/v1/Pago/add-pago",{monto,idPedido}
        ,{
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
       );

        return window.location.href = "/Assets/view/cliente/ordenes/Ordenes.html";

    }catch(err){
        
        console.log("Ocurrio un error intentar realizar el pago " + err);
    }
}
