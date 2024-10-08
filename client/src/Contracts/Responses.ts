export interface TransactionResponse {
    id: number,
    timestamp: Date,
    amount: number,
    description: string,
    account: string,
    category?: string
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

export interface AccountGroupResonse {
    itemId: number,
    accounts: AccountResponse[]
}

export interface AccountsResponse {
    groups: AccountGroupResonse[]
}

export interface CategoryCashFlowResponse {
    category: string,
    january: number,
    february: number,
    march: number,
    april: number,
    may: number,
    june: number,
    july: number,
    august: number,
    september: number,
    october: number,
    november: number,
    december: number,
    total: number
}

export interface CashFlowTotalsResponse {
    january: number,
    february: number,
    march: number,
    april: number,
    may: number,
    june: number,
    july: number,
    august: number,
    september: number,
    october: number,
    november: number,
    december: number,
    total: number
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
