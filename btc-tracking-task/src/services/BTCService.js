import axios from "axios";

export class BTCService {
  static async getRates() {
    try {
      const response = await axios.get(
        `https://api.coindesk.com/v1/bpi/currentprice.json`
      );
      if (response.status === 200) {
        return response.data;
      } else {
        throw new Error("Request failed for some reason");
      }
    } catch (error) {
      console.log(error);
    }
  }
}
