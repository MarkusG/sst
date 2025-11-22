import { TransactionResponse } from "../Contracts/Responses.ts";
import { AfterCategorizationAction } from "./AfterCategorizationAction.ts";
import { AfterAmountChangedAction } from "./AfterAmountChangedAction.ts";

export default interface TransactionRowProps {
  transaction: TransactionResponse;
  onCategoryUpdated: (after: AfterCategorizationAction) => any | Promise<any>;
  onAmountUpdated: (after: AfterAmountChangedAction) => any | Promise<any>;
}
