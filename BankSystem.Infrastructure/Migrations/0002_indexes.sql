CREATE INDEX idx_op_type ON history (operation_type_id)
CREATE INDEX idx_os_type ON history (operation_status_id)
CREATE INDEX idx_oc_id ON history (sender_customer_id)

CREATE UNIQUE INDEX idx_c_mail ON customer (email)
