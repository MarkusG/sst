export interface TransactionResponse {
    id: number,
    timestamp: Date,
    amount: number,
    description: string,
    account?: string,
    categorizations: CategorizationResponse[]
}

export interface CategorizationResponse {
    id: number,
    category: string,
    amount: number,
    position: number
}

export interface TransactionsResponse {
    page: number,
    pageCount: number,
    totalPages: number,
    totalCount: number,
    transactions: TransactionResponse[]
}

export interface CategoriesResponse {
    categories: string[]
}

export interface AccountResponse {
    id: number,
    name: string,
    availableBalance?: number,
    currentBalance?: number
}

export interface AccountGroupResponse {
    itemId?: number,
    accounts: AccountResponse[]
}

export interface AccountsResponse {
    groups: AccountGroupResponse[]
}

export interface CategoryCashFlowResponse {
    id: number,
    name: string,
    treeTotals: number[],
    categoryTotals: number[],
    yearTreeTotal: number,
    yearCategoryTotal: number,
    subcategories: CategoryCashFlowResponse[]
}

export interface CashFlowTotalsResponse {
    totals: number[],
    yearTotal: number
}

export interface CashFlowResponse {
    categories: CategoryCashFlowResponse[],
    totals: CashFlowTotalsResponse
}

export interface CategoryTreeEntryResponse {
    id: number,
    name: string,
    position: number,
    parentId: number,
    subcategories: CategoryTreeEntryResponse[]
}

export interface CategoryTreeResponse {
    categories: CategoryTreeEntryResponse[]
}
